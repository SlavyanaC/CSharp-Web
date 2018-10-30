namespace TorshiaWebApp.Controllers
{
    using SIS.MvcFramework;
    using Data;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            DbContext = new TorshiaDbContext();
        }

        protected TorshiaDbContext DbContext { get; set; }
    }
}
