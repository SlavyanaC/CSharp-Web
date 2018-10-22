namespace CakesWebApp.Controllers
{
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using Models;
    using ViewModels;

    public class OrdersControllers : BaseController
    {
        [HttpPost("/orders/add")]
        public IHttpResponse Add(int productId)
        {
            var userId = this.Db.Users.FirstOrDefault(x => x.Username == this.User)?.Id;
            if (userId == null)
            {
                return this.BadRequestError("Please login first.");
            }

            var lastUserOrder = this.Db.Orders.Where(u => u.UserId == userId)
               .OrderBy(u => u.Id).LastOrDefault();

            if (lastUserOrder == null)
            {
                lastUserOrder = new Order
                {
                    UserId = userId.Value,
                };
                this.Db.Orders.Add(lastUserOrder);
                this.Db.SaveChanges();
            }

            var orderProduct = new OrderProduct
            {
                OrderId = lastUserOrder.Id,
                ProductId = productId,
            };
            this.Db.OrderProducts.Add(orderProduct);
            this.Db.SaveChanges();

            return this.Redirect("/orders/byid?id=" + lastUserOrder.Id);
        }
        [HttpGet("/orders/byid")]
        public IHttpResponse GetById(int id)
        {
            var order = this.Db.Orders.FirstOrDefault(x => x.Id == id && x.User.Username == this.User);
            if (order == null)
            {
                return this.BadRequestError("Invalid order id.");
            }

            var lastOrderId = this.Db.Orders.Where(o => o.User.Username == this.User)
               .OrderByDescending(o => o.Id).Select(o => o.Id).FirstOrDefault();

            var viewModel = new OrderViewModel
            {
                Id = order.Id,
                Cakes = this.Db.OrderProducts.Where(op => op.OrderId == order.Id)
                    .Select(p => new CakeViewModel
                    {
                        Id = p.ProductId,
                        Name = p.Product.Name,
                        ImageUrl = p.Product.ImageUrl,
                        Price = p.Product.Price,
                    }).ToList(),
                IsShoppingCart = lastOrderId == order.Id
            };
            return this.View("OrderById", viewModel);
        }
    }
}