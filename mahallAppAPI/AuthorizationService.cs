using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace mahallAppAPI
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool isValidToken(string authToken, out JwtSecurityToken token)
        {
            //validate Token here
            var handler = new JwtSecurityTokenHandler();
            var a = authToken.Split(' ')[1];
            token = handler.ReadToken(a) as JwtSecurityToken;
            var userNameInToken = token.Subject;

            if (userNameInToken == "osman") // HARD CODED
                                            //TO DO:
                                            // Is User in Database ?
            {
                return true;
            }
            return false;
        }
    }
}
