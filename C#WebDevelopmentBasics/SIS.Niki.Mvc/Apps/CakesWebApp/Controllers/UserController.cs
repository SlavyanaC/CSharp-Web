namespace CakesWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using ViewModels;

    public class UserController : BaseController
    {
        [HttpGet("/user/profile")]
        public IHttpResponse Profile()
        {
            var viewModel = this.Db.Users.Where(u => u.Username == this.User)
                .Select(u => new ProfileViewModel
                {
                    Username = u.Username,
                    DateOfRegistration = u.DateOfRegistration,
                    OrdersCount = u.Orders.Count(),
                }).FirstOrDefault();

            if (viewModel == null)
            {
                return this.BadRequestError("Profile page not accessible for this user.");
            }

            return this.View("Profile", viewModel);
        }
    }
}
