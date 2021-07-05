using System.Threading.Tasks;
using DatingApp_Api.Enitites;
using Microsoft.AspNetCore.Identity;

namespace DatingApp_Api.Interface
{
    public interface ITokenService
    {
         Task<string> CreateToken(AppUser appUser);
    }
}