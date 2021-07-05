using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp_Api.Enitites;
using DatingApp_Api.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp_Api.Service
{
    public class TokenService:ITokenService
    {
         private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration configuration,UserManager<AppUser> userManager)
        {
           _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
            _userManager = userManager;
        }

        public async Task<string> CreateToken(AppUser user)
        {

            var claims = new List<Claim>()
            {
              new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.NameId,user.Id.ToString()),
             new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName,user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role =>new Claim(ClaimTypes.Role,role)));

            var Tokenhandler= new JwtSecurityTokenHandler();

            var descriptor = new  SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
            };

           var token =  Tokenhandler.CreateToken(descriptor);
           return Tokenhandler.WriteToken(token);

        }
    }
}