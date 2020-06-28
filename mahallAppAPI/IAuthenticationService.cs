namespace mahallAppAPI
{
    public interface IAuthenticationService
    {
        string generateJwtToken(UserInfo userInfo);
    }
}