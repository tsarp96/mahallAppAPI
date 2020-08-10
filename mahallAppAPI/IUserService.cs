using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mahallAppAPI
{
    public interface IUserService
    {
        bool AddUser(UserInfo user);
        UserInfo GetUserByName(string username);
        bool DeleteUserByName(string username);
    }
}
