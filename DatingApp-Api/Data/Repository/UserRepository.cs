using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp_Api.DTOs;
using DatingApp_Api.Enitites;
using DatingApp_Api.Helpers;
using DatingApp_Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.AppUsers
            .Where(x => x.UserName == username.ToLower())
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
            
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.AppUsers
            .Where(x => x.UserName == username.ToLower())
            .Include(p =>p.Photos)
            .SingleOrDefaultAsync();
        }

        public async Task<PageList<MemberDto>> GetMemberAsync(UserParams userParams)
        {
            var query = _context.AppUsers.AsQueryable();

            query = query.Where(x => x.UserName != userParams.CurrentUserName);
            query = query.Where(x => x.Gender == userParams.Gender);

            var MinDob = DateTime.Now.AddYears(-userParams.MaxAge -1);
            var MaxDob = DateTime.Now.AddYears(-userParams.MinAge);

            query = query.Where(x =>x.DateOfBirth >= MinDob && x.DateOfBirth <= MaxDob);

            query = userParams.OrderBy switch 
            {
                "created"=> query.OrderByDescending(u =>u.Create),
                _ => query.OrderByDescending(u =>u.LastActive)
            };

            return await PageList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking()
                ,userParams.PageSize,userParams.PageNumber);
        }

        public async Task<bool> SaveAllAsync()
        {
         return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}