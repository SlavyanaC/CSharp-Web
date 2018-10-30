using TorshiaWebApp.ViewModels.Home;
using TorshiaWebApp.ViewModels.Task;

namespace TorshiaWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User.Username);
            if (user != null)
            {
                var loggedInUserHomeViewModel = new LoggedInUserHomeViewModel
                {
                    Tasks = this.DbContext.Tasks.Select(t => new TaskViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Level = t.Sectors.Count(),
                    }).ToArray(),
                };
                return this.View("UserHomeIndex", loggedInUserHomeViewModel);
            }
            return this.View();
        }
    }
}
