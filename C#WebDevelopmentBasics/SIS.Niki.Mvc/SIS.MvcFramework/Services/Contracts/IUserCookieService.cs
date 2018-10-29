namespace SIS.MvcFramework.Services.Contracts
{
    public interface IUserCookieService
    {
        string GetUserCookie(MvcUserInfo user);

        MvcUserInfo GetUserData(string cookieContent);
    }
}
