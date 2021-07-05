using System;
using DatingApp_Api.Data;
using DatingApp_Api.Enitites;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp_Api.Extension
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityService(
            this IServiceCollection services,IConfiguration config)
        {

            services.AddIdentityCore<AppUser>(opt =>{
                opt.Password.RequireNonAlphanumeric = false;
                //opt.SignIn.RequireConfirmedEmail = false;
                //opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<DataContext>();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>{
                options.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["TokenKey"])),
                  ValidateIssuer = false,
                  ValidateAudience = false
                };
            });

            services.AddAuthorization(opt =>{
                opt.AddPolicy("RequiredAdminRole",policy =>policy.RequireRole("Admin"));
                opt.AddPolicy("ModeratorPhotoRole",policy =>policy.RequireRole("Moderator","Admin"));

            });

            return services;
        }

    }
}