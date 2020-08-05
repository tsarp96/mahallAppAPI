using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Xml.Xsl;
using Dapper;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        public UserInfo GetUserInfo(string username)
        {
            using (var conn = OpenConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return conn.Query<UserInfo>($"SELECT * from users where username='{username}'").FirstOrDefault();
            }
        }

        public bool AddUser(UserInfo user)
        {
            using (var conn = OpenConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try {
                    conn.Query<UserInfo>($"INSERT INTO users(username,password) VALUES('{user.Username}','{user.Password}')");
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool DeleteUser(string username)
        {
            using (var conn = OpenConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    conn.Query<UserInfo>($"DELETE FROM users WHERE username='{username}'");
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
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