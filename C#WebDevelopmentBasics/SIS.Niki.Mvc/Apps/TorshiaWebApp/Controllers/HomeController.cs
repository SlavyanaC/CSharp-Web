namespace TorshiaWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using ViewModels.Home;
    using ViewModels.Task;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            var user = this.DbContext.Users.FirstOrDefault(u => u.Username == this.User.Username);
            if (user != null)
            {
                var loggedInUserHomeViewModel = new LoggedInUserHomeViewModel
                {
                    Tasks = this.DbContext.Tasks.Where(t => !t.IsReported).Select(t => new TaskViewModel()
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
