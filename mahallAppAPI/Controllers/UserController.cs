using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mahallAppAPI.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/v1/register")]
        public ActionResult<String> Register([FromBody] RegisterRequest registerRequest  )
        {
            // TODO: 
            UserInfo user = new UserInfo();
            user.Username = registerRequest.UserName;
            user.Password = registerRequest.Password; // TODO : HASH amd SAVE
            // check if user already in DB
            if(_userService.GetUserByName(user.Username) != null)
            {
                return "User is already registered !";
            }
            _userService.AddUser(user);
            return "Başarıyla Kaydoldun Dostum :)";
        }
    }
}
