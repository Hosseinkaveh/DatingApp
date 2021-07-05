using Microsoft.AspNetCore.Identity;

namespace DatingApp_Api.Enitites
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public AppRole Role { get; set; }
        public AppUser User { get; set; }
    }
}