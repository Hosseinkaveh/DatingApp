using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DatingApp_Api.Data.Repository;
using DatingApp_Api.Interface;
using DatingApp_Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DatingApp_Api.Enitites;

namespace DatingApp_Api.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;

        }
        [HttpGet]
        public async Task<ActionResult<List<MemberDto>>> GetUsers()
        {
            return await _userRepository.GetMemberAsync();
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUsers(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            AppUser user = await _userRepository.GetUserByUsernameAsync(User.Identity.Name);

           _mapper.Map(memberUpdateDto,user);

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");


        }

    }

}