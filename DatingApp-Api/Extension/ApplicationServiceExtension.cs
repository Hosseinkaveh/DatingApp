using DatingApp_Api.Data;
using DatingApp_Api.Interface;
using DatingApp_Api.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp_Api.Extension
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(
          this  IServiceCollection services,IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options =>{
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            return services;

        }
    }
}