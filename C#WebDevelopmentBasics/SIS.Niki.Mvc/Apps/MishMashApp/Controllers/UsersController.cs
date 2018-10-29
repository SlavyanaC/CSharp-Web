namespace MishMashWebApp.Controllers
{
    using System;
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework.Services.Contracts;
    using Models;
    using Models.Enums;
    using ViewModels.User;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        public IHttpResponse Register()
        {
            return this.View("Register");
        }

        [HttpPost]
        public IHttpResponse Register(RegisterUserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return this.BadRequestError("Please provide valid username with length of 4 or more characters.");
            }

            if (this.DbContext.Users.Any(x => x.Username == model.Username.Trim()))
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

            var hashedPassword = this.hashService.Hash(model.Password);
            model.Password = hashedPassword;

            var user = model.To<User>();

            var role = UserRole.User;
            if (!this.DbContext.Users.Any())
            {
                role = UserRole.Admin;
            }

            user.Role = role;
            this.DbContext.Users.Add(user);

            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect("/users/login");
        }

        public IHttpResponse Login()
        {
            return this.View("Login");
        }

        [HttpPost]
        public IHttpResponse Login(LoginUserViewModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);
            var user = this.DbContext.Users.SingleOrDefault(u =>
                u.Username == model.Username && u.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);
            var cookie = new HttpCookie(".auth", cookieContent, 7);
            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }

        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth");
            cookie.Expire();;
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }
    }
}
