using DatingApp_Api.Data;
using DatingApp_Api.Data.Repository;
using DatingApp_Api.Helpers;
using DatingApp_Api.Interface;
using DatingApp_Api.Service;
using DatingApp_Api.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp_Api.Extension
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(
          this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IMessageRepository,MessageRespository>();
         services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            return services;

        }
    }
}