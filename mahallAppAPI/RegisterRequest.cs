using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mahallAppAPI
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string Gender { get; set; }
        public string AccountType { get; set; }
    }
}
