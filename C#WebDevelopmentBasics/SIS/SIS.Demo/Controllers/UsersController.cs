namespace SIS.Demo.Controllers
{
    using SIS.Framework.Controllers;
    using Framework.ActionResults.Contracts;
    using Framework.Attributes.Action;
    using Framework.Security;

    public class UsersController : Controller
    {
        public IActionResult Login()
        {
            this.SignIn(new IdentityUser { Username = "Pesho", Password = "123" });
            return this.View();
        }

        [Authorize]
        public IActionResult Authorized()
        {
            this.Model["username"] = this.Identity.Username;
            return this.View();
        }
    }
}
