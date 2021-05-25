using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatingApp_Api.Enitites;
using DatingApp_Api.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp_Api.Service
{
    public class TokenService:ITokenService
    {
         private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration)
        {
           _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }

        public string CreateToken(AppUser appUser)
        {

            var claims = new List<Claim>()
            {
              new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.NameId,appUser.Id.ToString()),
             new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName,appUser.UserName),
            };

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