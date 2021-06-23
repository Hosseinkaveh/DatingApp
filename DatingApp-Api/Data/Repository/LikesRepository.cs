using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;
using DatingApp_Api.Extension;
using DatingApp_Api.Helpers;
using DatingApp_Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Data.Repository
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<PageList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.AppUsers.OrderBy(x => x.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if (likesParams.predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);

            }
            if (likesParams.predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }
            var likedUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                City = user.City,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                Id = user.Id
            });

            return await PageList<LikeDto>.CreateAsync(likedUsers, likesParams.PageSize, likesParams.PageNumber);


        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.AppUsers
            .Include(x => x.LikedUser)
            .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}