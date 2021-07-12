using System;
using System.Linq;
using System.Threading.Tasks;
using API.SignalR;
using AutoMapper;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;
using DatingApp_Api.Extension;
using DatingApp_Api.Interface;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp_Api.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IHubContext<PresenceHub> _presenceHub;
        private readonly PresenceTracker _traker ;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessageHub(IMapper mapper, IUnitOfWork unitOfWork,
         IHubContext<PresenceHub> presenceHub, PresenceTracker traker)
        {
            _traker = traker;
            _presenceHub = presenceHub;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            

        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _unitOfWork.MessageRepository.
                GetMessageThread(Context.User.GetUsername(), otherUser);

            if (_unitOfWork.HasChanges()) await _unitOfWork.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        //     var httpContext = Context.GetHttpContext();
        //     var otherUser = httpContext.Request.Query["user"].ToString();
        //     var groupName = GetGroupName(Context.User.GetUsername(), otherUser);

        //     await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //    var group = await AddToGroup(groupName);
        //    await Clients.Group(groupName).SendAsync("UpdatedGroup",group);

        //     var message = await _unitOfWork.MessageRepository
        //     .GetMessageThread(Context.User.GetUsername(), otherUser);

        //     await Clients.Caller.SendAsync("ReceiveMessageThread", message);

        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipieantUserName.ToLower())
                throw new HubException("You cannot send message to yourself");

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipieantUserName);

            if (recipient == null) throw new HubException("Not found user");

            var message = new Message
            {
                Content = createMessageDto.Content,
                Sender = sender,
                Recipient = recipient,
                RecipientUsername = recipient.UserName,
                SenderUsername = sender.UserName
            };
            var groupName = GetGroupName(sender.UserName, recipient.UserName);

            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _traker.GetConnectionForUser(recipient.UserName);
                if(connections != null){
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageRecived",
                    new {username = sender.UserName,knownAs=sender.KnownAs});
                }

            }
            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete())
            {

                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }


        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
           var group = await RemoveFromMessageGroup();
           await Clients.Group(group.Name).SendAsync("UpdatedGroup",group);
            await base.OnDisconnectedAsync(ex);
        }
        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x =>x.ConnectionId == Context.ConnectionId);
            _unitOfWork.MessageRepository.RemoveConnection(connection);
            if(await _unitOfWork.Complete()) return group;

            throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}--{other}" : $"{other}--{caller}";

        }
    }
}