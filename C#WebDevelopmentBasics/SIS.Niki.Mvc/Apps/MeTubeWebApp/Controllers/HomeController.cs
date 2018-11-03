namespace MeTubeWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
    using ViewModels.Tubes;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var tubes = this.DbContext.Tubes.Select(t => t.To<TubeViewModel>()).ToArray();
                return this.View("UserHomeIndex", tubes);
            }

            return this.View();
        }
    }
}
