using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
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
        private readonly AppSettings _appSettings;
        
        public AuthController(IHashHelper hashHelper,IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            this._hashHelper = hashHelper;
            this._userRepository = userRepository;
            _appSettings = appSettings.Value;

        }

        private string generateJwtToken(UserInfo userInfo)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInfo.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

            var token = generateJwtToken(userInfo);

            return Ok(token);
        }
    }
}