using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
       
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _context = context;
            _mapper = mapper;

        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.username.ToLower())) return BadRequest("UserName is Taken");
             
             var user = _mapper.Map<AppUser>(registerDto);


            var hmac = new HMACSHA512();
            
                user.UserName = registerDto.username.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password));
                user.PasswordSalt = hmac.Key;
            
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                KnownAs = user.KnownAs,
                UserName = user.UserName,
                Token =_tokenService.CreateToken(user),
               // PhotoUrl = user.Photos.FirstOrDefault(x =>x.IsMain)?.Url,
                Gender = user.Gender
            };

        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(loginDto loginDto)
        {
            AppUser user = await _context.AppUsers.Include(x =>x.Photos).FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("UserName not valid");
            
            var hmac = new HMACSHA512(user.PasswordSalt);
            var pass = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < user.PasswordHash.Length; i++)
            {
                if (user.PasswordHash[i] != pass[i]) return Unauthorized("password not found");
            }

              return new UserDto
            {
                UserName = user.UserName,
                Token =_tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x =>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,

            };
            

        }
        public async Task<bool> UserExist(string UserName)
        {
            return await _context.AppUsers.AnyAsync(x => x.UserName == UserName);
        }
    }
}