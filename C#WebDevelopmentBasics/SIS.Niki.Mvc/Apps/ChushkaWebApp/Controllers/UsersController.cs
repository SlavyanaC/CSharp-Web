namespace ChushkaWebApp.Controllers
{
    using System;
    using SIS.HTTP.Cookies;
    using SIS.MvcFramework.Attributes;
    using System.Linq;
    using SIS.MvcFramework;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Services.Contracts;
    using Models;
    using Models.Enums;
    using ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        public IHttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public IHttpResponse Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Trim().Length < 4)
            {
                return this.BadRequestErrorWithView("Please provide valid username with length of 4 or more characters.");
            }

            if (this.DbContext.Users.Any(x => x.Username == model.Username.Trim()))
            {
                return this.BadRequestErrorWithView("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.BadRequestErrorWithView("Please provide password of length 6 or more.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestErrorWithView("Passwords do not match.");
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
            return this.View();
        }

        [HttpPost]
        public IHttpResponse Login(LoginViewModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);
            var user = this.DbContext.Users.FirstOrDefault(u =>
                u.Username == model.Username && u.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestErrorWithView("Invalid username or password.");
            }

            var mvcUser = new MvcUserInfo
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                Info = user.FullName,
            };

            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);
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
            cookie.Expire();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }
    }
}
