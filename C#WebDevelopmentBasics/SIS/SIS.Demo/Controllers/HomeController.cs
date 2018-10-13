namespace SIS.Demo.Controllers
{
    using Framework.ActionResults.Contracts;
    using SIS.Framework.Controllers;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
