namespace IRunesApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using SIS.HTTP.Requests.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            //if (!this.IsAuthenticated(request))
            //{
            //    return this.View();
            //}

            //var username = request.Session.GetParameter("username");
            //this.ViewBag["username"] = username.ToString();
            return this.View();

        }
    }
}
