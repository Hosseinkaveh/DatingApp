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

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipieantUserName.ToLower())
                return BadRequest("You cannot like yourself");

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipieantUserName);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Content = createMessageDto.Content,
                Sender = sender,
                Recipient = recipient,
                RecipientUsername = recipient.UserName,
                SenderUsername = sender.UserName
            };

            _unitOfWork.MessageRepository.AddMessage(message);
            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));


            return BadRequest("Faild to send Message");


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser([FromQuery]
        MessageParams messageParams)
        {
            messageParams.UserName = User.GetUsername();

            var messages = await _unitOfWork.MessageRepository.GetMessageForUser(messageParams);

            Response.AddPageInationHeaders(messages.PageNumber, messages.TotalPage
            , messages.PageSize, messages.TotalCount);

            return messages;

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var usernaem = User.GetUsername();

            var message = await _unitOfWork.MessageRepository.GetMessage(id);

            if (message.Sender.UserName != usernaem && message.Recipient.UserName != usernaem)
                return Unauthorized();

            if (message.Sender.UserName == usernaem) message.SenderDeleted = true;
            if (message.Recipient.UserName == usernaem) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _unitOfWork.MessageRepository.DeleteMessage(message);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("problem deleting the message");

        }
    }
}