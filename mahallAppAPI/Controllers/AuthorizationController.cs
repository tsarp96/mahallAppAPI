using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mahallAppAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
       [HttpGet]
       [CustomAuthorization]
        public ActionResult<IEnumerable<string>> Dinner()
        {
            return new string[] { "value1", "value2", "value3", "value4", "value5" };
        }
    }
}
