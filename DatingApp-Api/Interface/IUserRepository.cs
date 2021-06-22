using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;
using DatingApp_Api.Helpers;

namespace DatingApp_Api.Interface
{
    public interface IUserRepository
    {
        Task<PageList<MemberDto>> GetMemberAsync(UserParams userParams);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<MemberDto> GetMemberAsync(string username);
         void Update(AppUser user);
          Task<bool> SaveAllAsync();
        
    }
}