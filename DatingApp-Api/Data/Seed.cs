using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DatingApp_Api.Enitites;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Data
{
    public static class Seed
    {
        public static async Task UserSeed(DataContext context)
        {
            if(await context.AppUsers.AnyAsync()) return;

            var Data =await System.IO.File.ReadAllTextAsync("Data/seedingData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(Data);

            foreach(var user in users){
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.AppUsers.Add(user);
            }

            await context.SaveChangesAsync();

        }
        
    }
}