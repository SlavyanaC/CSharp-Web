namespace CHUSHKA.Web.Controllers
{
    using System;
    using System.Linq;
    using CHUSHKA.Models.Enums;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using ViewModels.Products;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var types = Enum.GetNames(typeof(ProductType)).ToList();
            var model = new ProductViewModel { Types = types };
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductViewModel model)
        {
            model.Type = model.Types.First();
            if (!ModelState.IsValid)
            {
                return this.View();
            }
            var productId = this.productService.Create(model.Name, model.Price, model.Description, model.Type);

            return this.RedirectToAction(nameof(Details), routeValues: productId);
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var model = this.productService.Details<ProductDetailsViewModel>(id);
            if (model == null)
                return this.Redirect("/");

            return this.View(model);
        }

        [Authorize]
        public IActionResult Order(string id)
        {
            var username = this.User.Identity.Name;
            var orderSucceeded = this.productService.Order(id, username);
            if (!orderSucceeded)
            {
                return this.RedirectToAction(nameof(Details), routeValues: id);
            }

            return this.Redirect(this.User.IsInRole("Admin") ? "/Orders/All" : "/");
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var model = this.productService.Details<ProductViewModel>(id);
            var types = Enum.GetNames(typeof(ProductType)).ToArray();
            model.Types = types;
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(ProductViewModel model)
        {
            model.Type = model.Types.First();
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var isEdited = this.productService.Edit(model.Id, model.Name, model.Price, model.Description, model.Type);
            if (!isEdited)
            {
                return this.Redirect("/");
            }

            return this.RedirectToAction(nameof(Details), routeValues: model.Id);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var model = this.productService.Details<ProductViewModel>(id);
            var types = Enum.GetNames(typeof(ProductType)).ToArray();
            model.Types = types;
            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(ProductViewModel model)
        {
            this.productService.Delete(model.Id);
            return this.Redirect("/");
        }
    }
}
