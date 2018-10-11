namespace CakesWebApp.Controllers
{
    using Data;
    using SIS.MvcFramework;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Db = new CakesDbContext();
        }

        protected CakesDbContext Db { get; }
    }
}
