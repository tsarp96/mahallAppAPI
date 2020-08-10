using mahallAppAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace mahallAppAPI.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private IConfiguration _configuration;

        public ImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void DeleteImageByName(string imageName)
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var con = new NpgsqlConnection(connStr);
            con.Open();
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            string cmd_str = "DELETE FROM user_image WHERE imagekey='"+ imageName + "'";
            cmd.CommandText = cmd_str;
            cmd.ExecuteNonQuery();
        }
        public static IDbConnection OpenConnection(string connStr)
        {
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            return conn;
        }
    }
}
