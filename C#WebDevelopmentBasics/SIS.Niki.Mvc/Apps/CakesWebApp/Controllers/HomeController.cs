namespace CakesWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using ViewModels;

    public class HomeController : BaseController
    {
        [HttpGet("/")]
        public IHttpResponse Index()
        {
            return this.View("Index");
        }

        [HttpGet("/hello")]
        public IHttpResponse HelloUser()
        {
            return this.View("HelloUser", new HelloUserInputModel { Username = this.User });
        }
    }
}
