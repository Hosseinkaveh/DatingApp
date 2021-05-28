using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DatingApp_Api.Data.Repository;
using DatingApp_Api.Interface;
using DatingApp_Api.DTOs;

namespace DatingApp_Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
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
       
    }

}