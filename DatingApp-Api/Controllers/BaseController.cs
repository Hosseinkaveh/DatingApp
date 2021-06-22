using DatingApp_Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp_Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController:ControllerBase
    {
        
    }
}