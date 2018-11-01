namespace ChushkaWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using ViewModels.Orders;

    public class OrdersController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var viewModel = this.DbContext.Orders.OrderBy(o => o.OrderedOn).Select(o => new OrderViewModel
            {
                Id = o.Id,
                Customer = o.Client.Username,
                Product = o.Product.Name,
                OrderedOn = o.OrderedOn.ToString("hh:mm dd/MM/yyyy"),
            }).ToArray();

            return this.View(viewModel);
        }
    }
}
