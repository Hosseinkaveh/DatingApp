using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp_Api.Data;
using DatingApp_Api.Enitites;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp_Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
          var host =  CreateHostBuilder(args).Build();
          using var scope = host.Services.CreateScope();
          var service = scope.ServiceProvider;
          try
          {
              DataContext context = service.GetRequiredService<DataContext>();
              var userManager = service.GetRequiredService<UserManager<AppUser>>();
              var roleManager = service.GetRequiredService<RoleManager<AppRole>>();
              await context.Database.MigrateAsync();
              await Seed.UserSeed(userManager,roleManager);
          }
          catch (System.Exception ex)
          {
              var logger = service.GetRequiredService<ILogger<Program>>();
              logger.LogError(ex,"An Error occurred during Migration");
          }
          await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
