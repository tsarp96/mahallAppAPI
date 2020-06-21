using System;
using Microsoft.AspNetCore.Mvc;

namespace mahallAppAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHashHelper _hashHelper;
        private readonly IUserRepository _userRepository;
        
        public AuthController(IHashHelper hashHelper,IUserRepository userRepository)
        {
            this._hashHelper = hashHelper;
            this._userRepository = userRepository;
        }
        
        [HttpPost]
        public IActionResult Auth([FromBody] AuthRequest authRequest)
        {
            var isValidRequest = String.IsNullOrEmpty(authRequest.UserName) || String.IsNullOrEmpty(authRequest.Password);
            if (isValidRequest)
            {
                return BadRequest();
            }

            var hash = _hashHelper.Hash(authRequest.Password);

            var userInfo = _userRepository.GetUserInfo(authRequest.UserName, "123");

            if (userInfo == null)
            {
                return Unauthorized();
            }

            return Ok(userInfo);
        }
    }
}