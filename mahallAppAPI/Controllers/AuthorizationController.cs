using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mahallAppAPI.Controllers
{
   
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
       [HttpGet]
       [CustomAuthorization]
       [Route("api/v1/dinner")]
        public ActionResult<String> Dinner()
        {
            // TODO: return user name
            return User.Identities.Select(x => x.Name).LastOrDefault() ;
        }
    }
}
