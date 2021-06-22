using System;
using System.Threading.Tasks;
using DatingApp_Api.Extension;
using DatingApp_Api.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp_Api.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var resultContext = await next();
           
           if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

           var userId = resultContext.HttpContext.User.GetUserId();
           var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
           var user = await repo.GetUserByIdAsync(userId);
           user.LastActive = DateTime.Now;
           await repo.SaveAllAsync();


        }
    }
}