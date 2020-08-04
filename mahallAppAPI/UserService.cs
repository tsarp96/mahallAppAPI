using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace mahallAppAPI
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        
        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public bool AddUser(UserInfo user)
        {
            if (_userRepository.AddUser(user))
            {
                return true;
            }
            return false ;
        }

        public UserInfo GetUserByName(string username)
        {
            return _userRepository.GetUserInfo(username);
        }
    }
}
