namespace MishMashWebApp.Controllers
{
    using SIS.MvcFramework;
    using Data;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            DbContext = new MishMashDbContext();
        }

        protected MishMashDbContext DbContext { get; set; }
    }
}
