namespace ChushkaWebApp.Controllers
{
    using SIS.MvcFramework;
    using Data;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.DbContext = new ChushkaDbContext();
        }

        protected ChushkaDbContext DbContext { get; set; }
    }
}
