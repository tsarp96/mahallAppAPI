using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace mahallAppAPI
{
    interface IAuthorizationService
    {
        bool isValidToken(string authToken, out JwtSecurityToken token);
    }
}
