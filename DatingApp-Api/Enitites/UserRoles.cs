using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp_Api.Enitites
{
    public class AppRole:IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
        
    }
}