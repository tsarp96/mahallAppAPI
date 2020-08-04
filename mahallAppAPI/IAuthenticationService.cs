namespace mahallAppAPI
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(UserInfo userInfo);
    }
}