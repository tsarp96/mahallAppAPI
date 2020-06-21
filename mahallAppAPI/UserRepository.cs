using System.Data;
using System.Linq;
using System.Xml.Xsl;
using Dapper;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;

namespace mahallAppAPI
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public UserInfo GetUserInfo(string username, string password)
        {
            using (var conn = OpenConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return conn.Query<UserInfo>($"SELECT * from users where username='{username}' and password='{password}'").FirstOrDefault();
            }
        }
        
        public static IDbConnection OpenConnection(string connStr)  
        {  
            var conn = new NpgsqlConnection(connStr);  
            conn.Open();  
            return conn;  
        }  
        

    }
}