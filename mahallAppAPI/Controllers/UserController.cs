using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

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

        [HttpPost]
        [CustomAuthorization]
        [Route("api/v1/delete")]
        public ActionResult<String> Delete([FromBody] DeleteRequest deleteRequest)
        {
            UserInfo user = new UserInfo();
            user.Username = deleteRequest.UserName;
            if (_userService.GetUserByName(user.Username) == null)
            {
                return "User Not Found !";
            }
            _userService.DeleteUserByName(user.Username);
            return "User is deleted !";
        }
    }
}
