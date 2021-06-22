using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DatingApp_Api.Data.Repository;
using DatingApp_Api.Interface;
using DatingApp_Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DatingApp_Api.Enitites;
using DatingApp_Api.Extension;
using System.Linq;
using Microsoft.AspNetCore.Http;
using DatingApp_Api.Helpers;

namespace DatingApp_Api.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoservice;
        public UsersController(IUserRepository userRepository, IMapper mapper,IPhotoService photoservice)
        {
            _mapper = mapper;
            _photoservice = photoservice;
            _userRepository = userRepository;

        }
        [HttpGet]
        public async Task<ActionResult<List<MemberDto>>> GetUsers([FromQuery]UserParams param)
        {
            var CurrentUser = await  _userRepository.GetUserByUsernameAsync(User.GetUsername());
            param.CurrentUserName = CurrentUser.UserName;
            
            if(string.IsNullOrEmpty(param.Gender))
            param.Gender = CurrentUser.Gender == "male" ? "female":"male";


            var users = await _userRepository.GetMemberAsync(param);

            Response.AddPageInationHeaders(users.PageNumber,users.TotalPage,users.PageSize,users.TotalCount);
            return Ok(users);
        }

        [HttpGet("{username}",Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUsers(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            AppUser user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

           _mapper.Map(memberUpdateDto,user);

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");


        }
          [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
           
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result =await _photoservice.AddPhotoAsync(file);

            if(result.Error != null) 
            return BadRequest(result.Error.Message);

                var photo = new Photo {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,
                };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;

            }
            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
               return CreatedAtRoute("GetUser",new {username= user.UserName},_mapper.Map<PhotoDto>(photo));
         //  return _mapper.Map<PhotoDto>(photo);
            }

            return BadRequest("Problem adding photo");


        }

        [HttpPut("set-main-photo/{photoid}")]
        public async Task<ActionResult>  SetMainPhoto(int photoid)
        {
            var user =await _userRepository.GetUserByUsernameAsync(User.GetUsername());

           var photo =  user.Photos.FirstOrDefault(x =>x.Id == photoid);

           if (photo.IsMain) return BadRequest("This is already your main photo");

           var currentmain = user.Photos.FirstOrDefault(x =>x.IsMain);
           if(currentmain != null) currentmain.IsMain =false;
           photo.IsMain = true;
           
           if(await _userRepository.SaveAllAsync()) return NoContent();
           return BadRequest("failed to set main pohto");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
              var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

              var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

              if(photo == null) return NotFound();

              if(photo.IsMain) return BadRequest("you can not delete main photo");

              if(photo.PublicId !=null)
              {
                  var result = await _photoservice.DeletePhotoAsync(photo.PublicId);
                  if(result.Error != null) return BadRequest(result.Error.Message);
              }
              user.Photos.Remove(photo);
              if(await _userRepository.SaveAllAsync()) return Ok();
              
              return BadRequest("Faild to delete photo");

        }
    }

}