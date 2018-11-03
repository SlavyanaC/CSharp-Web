namespace FDMCWebApp.Controllers
{
    using SIS.MvcFramework;
    using Data;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            DbContext = new FDMCDbContext();
        }

        protected FDMCDbContext DbContext { get; set; }
    }
}
