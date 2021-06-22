using System.Security.Claims;

namespace DatingApp_Api.Extension
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal User)
        {
           return User.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal User)
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
           return id;
        }
        
    }
}