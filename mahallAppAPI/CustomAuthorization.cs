using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace mahallAppAPI
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            // OnAuthorization methoduna başka parametre ekleyemedim..
            AuthorizationService authorizationService = new AuthorizationService();

            if (filterContext != null)
            {
                Microsoft.Extensions.Primitives.StringValues authTokens;
                var hasAuthorizationHeader = filterContext.HttpContext.Request.Headers.TryGetValue("authorization", out authTokens);
                
                var token = authTokens.FirstOrDefault();

                if (token != null && authTokens.ToString().Trim().StartsWith("Bearer"))
                {
                    string authToken = token;
                   
                        if (authorizationService.isValidToken(authToken, out var tokenn)) 
                        {
                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, tokenn.Subject));
                            var id = new ClaimsIdentity(claims);
                            filterContext.HttpContext.User.AddIdentity(id);
                            return;
                        }
                        else
                        {
                            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                            filterContext.Result = new JsonResult("NotAuthorized")
                            {
                                Value = new
                                {
                                    Status = "Error",
                                    Message = "Invalid Token"
                                },
                            };
                        }
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                    filterContext.Result = new JsonResult("Please Provide authToken")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = "Please Provide authToken"
                        },
                    };
                }
            }
        }

    }
}
