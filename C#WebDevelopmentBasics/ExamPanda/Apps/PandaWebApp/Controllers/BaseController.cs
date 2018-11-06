namespace PandaWebApp.Controllers
{
    using SIS.MvcFramework;
    using Data;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.DbContext = new PandaDbContext();
        }

        protected PandaDbContext DbContext { get; set; }
    }
}
