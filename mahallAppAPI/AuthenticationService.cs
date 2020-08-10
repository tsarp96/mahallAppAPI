using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mahallAppAPI
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _config;
        public AuthenticationService(IOptions<AppSettings> appSettings, IConfiguration config)
        {
            this._appSettings = appSettings.Value;
            this._config = config;
        }

        public string GenerateJwtToken(UserInfo userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
               _config["Jwt:Issuer"],
               claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                    new Claim("userId", userInfo.Id)
                },
               expires: DateTime.Now.AddMinutes(120),
               signingCredentials: credentials); ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
