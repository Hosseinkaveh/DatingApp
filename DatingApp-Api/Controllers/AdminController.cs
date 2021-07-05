using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp_Api.Enitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Controllers
{
    public class AdminController:BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult<IEnumerable<AppUser>>>  GetUsersWithRoles()
        {
           var user =await _userManager.Users
           .Include(u =>u.UserRoles)
           .OrderBy(u =>u.UserName)
           .Select(u =>new {
               u.Id,
               Username = u.UserName,
               Roles = u.UserRoles.Select(r =>r.Role.Name).ToList()
           })
           .ToListAsync();
           return Ok(user);
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username,[FromQuery]string roles)
        {
            var selectRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if(user == null) return NotFound("Could not found user");

            var userRoles = await _userManager.GetRolesAsync(user);

           var result =   await  _userManager.AddToRolesAsync(user,selectRoles.Except(userRoles));

           if(!result.Succeeded) return BadRequest("Failed to add to role");

            result = await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectRoles));

           if(!result.Succeeded) return BadRequest("Failed to remove from role");

           return Ok(await _userManager.GetRolesAsync(user));


        }

         [Authorize(Policy = "ModeratorPhotoRole")]
        [HttpGet("photo-to-moderator")]
        public ActionResult GetPhotosforModerator()
        {
            return Ok();
        }
        
    }
}