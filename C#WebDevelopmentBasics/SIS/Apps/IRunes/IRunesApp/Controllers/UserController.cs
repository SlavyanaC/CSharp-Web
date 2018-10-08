namespace IRunesApp.Controllers
{
    using System;
    using System.Linq;
    using Services;
    using SIS.HTTP.Cookies;
    using SIS.WebServer.Results;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using IRunesModels;

    public class UserController : BaseController
    {
        private readonly HashService hashService;

        public UserController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Login(IHttpRequest request) => this.View();

        public IHttpResponse PostLogin(IHttpRequest request)
        {
            var usernameOrEmail = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();
            var passwordHash = this.hashService.Hash(password);
            var user = this.Db.Users.FirstOrDefault(u => (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && u.HashedPassword == passwordHash);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!");
            }

            request.Session.AddParameter("username", usernameOrEmail);

            var cookieContent = this.UserCookieService.GetUserCookie(usernameOrEmail);

            var response = new RedirectResult("/Home/Index");

            var cookie = new HttpCookie(".auth-irunes", cookieContent, 7) { IsHttpOnly = true };
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse Register(IHttpRequest request) => this.View();

        public IHttpResponse PostRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            var email = request.FormData["email"].ToString();

            var tupleInvalidFormAndMessage = this.ValidateRegistrationData(username, password, confirmPassword);
            if (tupleInvalidFormAndMessage.Item1)
            {
                return this.BadRequestError(tupleInvalidFormAndMessage.Item2);
            }

            var hashedPassword = this.hashService.Hash(password);

            try
            {
                AddUserToDb(username, hashedPassword, email);
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.PostLogin(request);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return this.View("/Home/Index");
            }

            request.Session.ClearParameters();
            var cookie = request.Cookies.GetCookie(".auth-irunes");
            cookie.Delete();

            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);
            return response;
        }

        private void AddUserToDb(string username, string hashedPassword, string email)
        {
            var user = new User
            {
                Username = username,
                HashedPassword = hashedPassword,
                Email = email,
            };

            this.Db.Users.Add(user);
            this.Db.SaveChanges();
        }

        private Tuple<bool, string> ValidateRegistrationData(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return new Tuple<bool, string>(true, "Please provide valid username with length of 4 or more characters.");
            }

            if (this.Db.Users.Any(u => u.Username == username))
            {
                return new Tuple<bool, string>(true, "User with the same name already exists");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return new Tuple<bool, string>(true, "Password must be more than 5 characters");
            }

            if (password != confirmPassword)
            {
                return new Tuple<bool, string>(true, "Passwords do not match");
            }

            return new Tuple<bool, string>(false, "All data is valid");
        }
    }
}
