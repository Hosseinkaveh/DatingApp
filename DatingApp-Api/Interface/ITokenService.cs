using DatingApp_Api.Enitites;

namespace DatingApp_Api.Interface
{
    public interface ITokenService
    {
         string CreateToken(AppUser appUser);
    }
}