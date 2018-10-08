using SIS.HTTP.Requests.Contracts;

namespace IRunesApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            // TODO: Check if authenticated
            if (this.IsAuthenticated(request))
            {
                var username = request.Session.GetParameter("username");
                this.ViewBag["username"] = username.ToString();
                return this.View("IndexLogged-in");
            }

            return this.View();
        }
    }
}
