namespace MishMashWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;

    public class HomeController : BaseController
    {
        [HttpGet("/home/index")]
        public IHttpResponse Index()
        {
            return this.View("HomeIndex");
        }

        [HttpGet("/")]
        public IHttpResponse RootIndex()
        {
            return this.View("HomeIndex");
        }
    }
}
