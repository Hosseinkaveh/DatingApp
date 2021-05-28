using DatingApp_Api.Data;
using DatingApp_Api.Enitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp_Api.Controllers
{
    public class buggyController : BaseController
    {
        private readonly DataContext _context;
        public buggyController(DataContext context)
        {
            _context = context;

        }

      [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var things = _context.AppUsers.Find(-1);
            if(things == null) return NotFound();
            return things;

        }

         [Authorize]
         [HttpGet("auth")]
         public ActionResult<string> GetSecText()
         {
             return "security text";

         }
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.AppUsers.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest();
        }

    }
}