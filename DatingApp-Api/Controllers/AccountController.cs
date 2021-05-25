using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp_Api.Data;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;
using DatingApp_Api.Interface;
using DatingApp_Api.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;

        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.UserName.ToLower())) return BadRequest("UserName is Taken");

            var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                Email = registerDto.Email,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token =_tokenService.CreateToken(user)
            };

        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(loginDto loginDto)
        {
            AppUser user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("UserName not valid");
            
            var hmac = new HMACSHA512(user.PasswordSalt);
            var pass = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < user.Password.Length; i++)
            {
                if (user.Password[i] != pass[i]) return Unauthorized("password not found");
            }

              return new UserDto
            {
                UserName = user.UserName,
                Token =_tokenService.CreateToken(user)
            };
            

        }
        public async Task<bool> UserExist(string UserName)
        {
            return await _context.AppUsers.AnyAsync(x => x.UserName == UserName);
        }
    }
}