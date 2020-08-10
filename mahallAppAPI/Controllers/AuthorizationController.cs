using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using mahallAppAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Npgsql;

namespace mahallAppAPI.Controllers
{
   
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IS3Service _service;
        static string connStr = "User ID=postgres;Password=da8566ffb75a1fa378d5b73ad878829c;Server=keosdesign.net;Port=13876;Database=mahall_test;Integrated Security=true;Pooling=true;";

        public static bool createTable(string connStr) // The code inside in that method will be change according to needs. 
        {
            using var con = new NpgsqlConnection(connStr);
            con.Open();
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"CREATE TABLE User_Image(
                      userId INTEGER,
                      id SERIAL PRIMARY KEY, 
                      imageKey VARCHAR(255)
                    );";
            cmd.ExecuteNonQuery();
            return true;
        }
        public AuthorizationController(IS3Service service)
        {
            _service = service;
        }


        public static bool insert(string userid, string imageKey)
        {
            using var con = new NpgsqlConnection(connStr);
            con.Open();
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            string cmd_str = "INSERT INTO user_image(userid,imagekey) VALUES('"+ userid +"','" + imageKey + "')";
            cmd.CommandText = cmd_str;
            cmd.ExecuteNonQuery();
            return true;
        }


       [HttpGet]
       [CustomAuthorization]
       [Route("api/v1/dinner")]
        public async Task<List<string>> uploadImage(IFormFile file)
        {

            long size = file.Length;
            var handler = new JwtSecurityTokenHandler();
            StringValues authTokens;
            HttpContext.Request.Headers.TryGetValue("authorization", out authTokens);
            var a = authTokens.ToString().Split(' ')[1];
            var token = handler.ReadToken(a) as JwtSecurityToken;

            //TO DO: DB DE USER İLE IMAGEID TABLOSUNU AYARLA
            insert(token.Actor, file.FileName);


            await _service.UploadFileAsync("hadibeoglum", file); // userid kullanabilirsin kullancılara özel ad vermek istersen.

            return new List<string>{ token.Actor.ToString() , token.Subject.ToString() , "Uploaded a file !"};
        }


        //public void getImage()
        //{

        //}

        //public void deleteImage()
        //{

        //}


    }
}
