namespace CHUSHKA.Services.Contracts
{
    using System.Threading.Tasks;
    using Models;

    public interface IAccountService
    {
        Task<bool> Register(string username, string password, string confirmPassword, string email, string fullName);

        Task<bool> Login(string username, string password);

        Task<User> GetUser(string username);

        void Logout();
    }
}
