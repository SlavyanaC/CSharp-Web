namespace ChushkaWebApp.Controllers
{
    using System.Linq;
    using SIS.MvcFramework;
    using ViewModels.Products;

    using SIS.HTTP.Responses.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (!this.User.IsLoggedIn)
            {
                return this.View();
            }

            var productsViewModel = this.DbContext.Products
                .Select(p => p.To<ProductViewModel>())
                .ToArray();

            return this.View("UserHomeIndex", productsViewModel);
        }
    }
}
