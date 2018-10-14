namespace CakesWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;
    using System.Collections.Generic;
    using SIS.MvcFramework.Attributes;

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
            return this.View("HelloUser", new
                Dictionary<string, string>
                {
                    {"Username", this.User}
                });
        }
    }
}
