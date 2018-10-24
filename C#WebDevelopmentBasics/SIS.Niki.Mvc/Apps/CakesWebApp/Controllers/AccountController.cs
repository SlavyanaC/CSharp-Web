namespace CakesWebApp.Controllers
{
    using System;
    using System.Linq;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Services.Contracts;
    using SIS.MvcFramework.Attributes;
    using Models;
    using ViewModels.Account;

    public class AccountController : BaseController
    {
        private readonly IHashService hashService;

        public AccountController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/account/register")]
        public IHttpResponse Register()
        {
            return this.View("Register");
        }

        [HttpPost("/account/register")]
        public IHttpResponse DoRegister(DoRegisterViewModel model)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return this.BadRequestError("Please provide valid username with length of 4 or more characters.");
            }

            if (this.Db.Users.Any(x => x.Username == model.Username.Trim()))
            {
                return this.BadRequestError("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestError("Please provide password of length 6 or more.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestError("Passwords do not match.");
            }

            // Hash password
            var hashedPassword = this.hashService.Hash(model.Password);

            // Create user
            var user = new User
            {
                Name = model.Username.Trim(),
                Username = model.Username.Trim(),
                Password = hashedPassword,
            };
            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            // TODO: Login

            return this.Redirect("/");
        }

        [HttpGet("/account/login")]
        public IHttpResponse Login()
        {
            return this.View("Login");
        }

        [HttpPost("account/login")]
        public IHttpResponse DoLogin(DoLoginViewModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);

            var user = this.Db.Users.FirstOrDefault(x =>
                x.Username == model.Username &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password.");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth", cookieContent, 7) { IsHttpOnly = true };
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        [HttpGet("/account/logout")]
        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }
    }
}
