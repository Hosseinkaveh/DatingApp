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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;

        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.username.ToLower())) return BadRequest("UserName is Taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.username.ToLower();

            var result =   await _userManager.CreateAsync(user,registerDto.password);

            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user,"Member");

            if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return new UserDto
            {
                KnownAs = user.KnownAs,
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Gender = user.Gender
            };

        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(loginDto loginDto)
        {
            AppUser user = await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("UserName not valid");

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
            if(!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,

            };


        }
        public async Task<bool> UserExist(string UserName)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == UserName);
        }
    }
}