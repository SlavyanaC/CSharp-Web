﻿namespace MeTubeWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Services.Contracts;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework;
    using Models;
    using ViewModels.Tubes;
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

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Trim().Length < 4)
            {
                return this.BadRequestErrorWithView("Please provide valid email with length of 4 or more characters.");
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

            model.Password = this.hashService.Hash(model.Password);
            var user = model.To<User>();
            this.DbContext.Users.Add(user);

            this.DbContext.SaveChanges();

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

            var user = this.DbContext.Users.FirstOrDefault(x =>
                x.Username == model.Username.Trim() &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestErrorWithView("Invalid username or password.");
            }

            var mvcUser = new MvcUserInfo
            {
                Username = user.Username,
                //TODO: This might be dangerous
                Info = user.Email.Replace("@", "&#64;"),
            };
            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);

            var cookie = new HttpCookie(".auth", cookieContent, 7) { IsHttpOnly = true };
            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Profile()
        {
            var userId = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User.Username)?.Id;
            var tubes = this.DbContext.Tubes.Where(t => t.UploaderId == userId)
                .Select(t => t.To<TubeViewModel>())
                .ToArray();
            return this.View(tubes);
        }

        [Authorize]
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
