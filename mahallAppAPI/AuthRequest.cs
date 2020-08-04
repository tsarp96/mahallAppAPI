using System.ComponentModel.DataAnnotations;

namespace mahallAppAPI
{
    public class AuthRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}