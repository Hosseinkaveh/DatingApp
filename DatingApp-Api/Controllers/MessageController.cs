using Microsoft.AspNetCore.Mvc;
using DatingApp_Api.Interface;
using DatingApp_Api.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DatingApp_Api.Extension;
using DatingApp_Api.Enitites;
using AutoMapper;
using DatingApp_Api.Helpers;
using System.Collections.Generic;

namespace DatingApp_Api.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipieantUserName.ToLower())
                return BadRequest("You cannot like yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipieantUserName);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Content = createMessageDto.Content,
                Sender = sender,
                Recipient = recipient,
                RecipientUsername = recipient.UserName,
                SenderUsername = sender.UserName
            };

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));


            return BadRequest("Faild to send Message");


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser([FromQuery]
        MessageParams messageParams)
        {
            messageParams.UserName = User.GetUsername();

            var messages = await _messageRepository.GetMessageForUser(messageParams);

            Response.AddPageInationHeaders(messages.PageNumber, messages.TotalPage
            , messages.PageSize, messages.TotalCount);

            return messages;

        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _messageRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var usernaem = User.GetUsername();

            var message = await _messageRepository.GetMessage(id);

            if(message.Sender.UserName != usernaem && message.Recipient.UserName != usernaem)
            return Unauthorized();

            if(message.Sender.UserName == usernaem) message.SenderDeleted = true;
            if(message.Recipient.UserName == usernaem) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
            _messageRepository.DeleteMessage(message);

            if(await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("problem deleting the message");
             
        }
    }
}