using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;

namespace DatingApp_Api.Interface
{
    public interface IUserRepository
    {
        Task<List<MemberDto>> GetMemberAsync();
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<MemberDto> GetMemberAsync(string username);
         void Update(AppUser user);
          Task<bool> SaveAllAsync();
        
    }
}