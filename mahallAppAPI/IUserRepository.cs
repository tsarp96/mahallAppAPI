namespace mahallAppAPI
{
    public interface IUserRepository
    {
        UserInfo GetUserInfo(string username, string password);
        UserInfo GetUserInfo(string username);
        bool AddUser(UserInfo user);
    }
}