using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DatingApp_Api.Enitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Data
{
    public static class Seed
    {
        public static async Task UserSeed(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            if(await userManager.Users.AnyAsync()) return;

            var Data =await System.IO.File.ReadAllTextAsync("Data/seedingData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(Data);
            if(users == null) return;

            var roles = new List<AppRole>{
                new AppRole{Name="Member"},
                new AppRole{Name="Moderator"},
                new AppRole{Name="Admin"},
            };

            foreach(var role in roles){

                await roleManager.CreateAsync(role);
            }

            foreach(var user in users){

                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user,"Pa$$w0rd");
                await userManager.AddToRoleAsync(user,"Member");
            }

            var admin = new AppUser{
                UserName= "Admin"
            };
                 await userManager.CreateAsync(admin,"Pa$$w0rd");
                await userManager.AddToRolesAsync(admin,new[] {"Admin","Moderator"});



        }
        
    }
}