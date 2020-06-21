namespace mahallAppAPI
{
    public interface IUserRepository
    {
        UserInfo GetUserInfo(string username, string password);
    }
}