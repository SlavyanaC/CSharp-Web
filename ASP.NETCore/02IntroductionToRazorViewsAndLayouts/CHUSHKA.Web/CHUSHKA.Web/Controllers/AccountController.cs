namespace CHUSHKA.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using Models.Account;

    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = this.accountService.Login(model.Username, model.Password);

            if (!result.Result)
            {
                return this.View();
            }

            return this.Redirect("/");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = this.accountService.Register(model.Username, model.Password, model.ConfirmPassword, model.Email, model.FullName);

            if (!result.Result)
            {
                return this.View();
            }

            return this.Redirect(nameof(Login));
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.accountService.Logout();
            return this.Redirect("/");
        }
    }
}