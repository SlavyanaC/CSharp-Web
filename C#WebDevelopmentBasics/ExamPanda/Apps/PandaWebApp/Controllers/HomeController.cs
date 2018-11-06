namespace PandaWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses;
    using Models.Enums;
    using ViewModels.Home;
    using ViewModels.Packages;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var userHomeViewModel = this.GetUserHomeIndexModel();

                return this.View("Home/UserHomeIndex", userHomeViewModel);
            }

            return this.View();
        }

        private UserHomeViewModel GetUserHomeIndexModel()
        {
            var userHomeViewModel = new UserHomeViewModel();

            var pending = this.DbContext.Packages
                .Where(p => p.Recipient.Username == this.User.Username && p.Status == Status.Pending)
                .Select(p => new PackageHomeIndex
                {
                    Id = p.Id,
                    Description = p.Description
                })
                .ToArray();

            var shipped = this.DbContext.Packages
                .Where(p => p.Recipient.Username == this.User.Username && p.Status == Status.Shipped)
                .Select(p => new PackageHomeIndex
                {
                    Id = p.Id,
                    Description = p.Description
                })
                .ToArray();

            var delivered = this.DbContext.Packages
                .Where(p => p.Recipient.Username == this.User.Username && p.Status == Status.Delivered)
                .Select(p => new PackageHomeIndex
                {
                    Id = p.Id,
                    Description = p.Description
                })
                .ToArray();

            userHomeViewModel.Pending = pending;
            userHomeViewModel.Shipped = shipped;
            userHomeViewModel.Delivered = delivered;

            return userHomeViewModel;
        }
    }
}
