namespace CHUSHKA.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult All()
        {
            var model = this.orderService.All<OrderViewModel>();
            return View(model);
        }
    }
}