namespace MeTubeWebApp.Controllers
{
    using SIS.MvcFramework;
    using Data;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            DbContext = new MeTubeDbContext();
        }

        protected MeTubeDbContext DbContext { get; set; }
    }
}
