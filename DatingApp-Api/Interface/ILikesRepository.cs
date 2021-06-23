using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;
using DatingApp_Api.Helpers;

namespace DatingApp_Api.Interface
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams);

    }
}