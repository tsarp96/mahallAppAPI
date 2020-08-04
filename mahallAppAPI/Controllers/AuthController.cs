using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace mahallAppAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHashHelper _hashHelper;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _config;

        public AuthController(IHashHelper hashHelper, IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            this._hashHelper = hashHelper;
            this._userRepository = userRepository;
            this._authenticationService = authenticationService;
        }

        [HttpPost]
        [AllowAnonymous]
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

            var token = _authenticationService.GenerateJwtToken(userInfo);

            return Ok(token);
        }

    }
}