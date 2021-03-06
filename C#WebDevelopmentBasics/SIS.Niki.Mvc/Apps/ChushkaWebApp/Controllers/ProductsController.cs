﻿namespace ChushkaWebApp.Controllers
{
    using System;
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework;
    using ViewModels.Products;
    using Models;
    using Models.Enums;

    public class ProductsController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(string id)
        {
            var product = this.DbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequestError($"Product with id {id} not found.");
            }

            var productViewModel = product.To<ProductViewModel>();
            return this.View(productViewModel);
        }

        [Authorize]
        public IHttpResponse Order(string id)
        {
            var client = this.DbContext.Users.FirstOrDefault(c => c.Username == this.User.Username);
            var product = this.DbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequestError($"Product with id {id} not found.");
            }

            var order = new Order()
            {
                ClientId = client.Id,
                ProductId = product.Id,
                OrderedOn = DateTime.UtcNow,
            };
            this.DbContext.Orders.Add(order);
            this.DbContext.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(ProductViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Type) || model.Price <= 0 || string.IsNullOrWhiteSpace(model.Description))
            {
                return this.BadRequestErrorWithView("All fields ar required.");
            }
            model.Id = string.Empty;
            var product = model.To<Product>();
            product.Id = null;
            product.Type = Enum.Parse<ProductType>(model.Type);
            this.DbContext.Products.Add(product);
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(string id)
        {
            var product = this.DbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequestError($"Product with id {id} not found.");
            }

            var productViewModel = product.To<ProductViewModel>();
            return this.View(productViewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(ProductViewModel model)
        {
            var product = this.DbContext.Products.FirstOrDefault(p => p.Id == model.Id);

            if (product == null)
            {
                return this.BadRequestError("Product not found.");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Type) || model.Price <= 0 || string.IsNullOrWhiteSpace(model.Description))
            {
                return this.BadRequestErrorWithView("All fields ar required.");
            }

            product.Name = model.Name;
            product.Type = Enum.Parse<ProductType>(model.Type);
            product.Price = model.Price;
            product.Description = model.Description;

            this.DbContext.Products.Update(product);
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect($"/products/details?id={model.Id}");
        }

        [Authorize("Admin")]
        public IHttpResponse Delete(string id)
        {
            var product = this.DbContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return this.BadRequestError($"Product with id {id} not found.");
            }

            var productViewModel = product.To<ProductViewModel>();
            return this.View(productViewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Delete(ProductViewModel model)
        {
            var product = this.DbContext.Products.FirstOrDefault(p => p.Id == model.Id);

            if (product == null)
            {
                return this.BadRequestError("Product not found.");
            }

            this.DbContext.Products.Remove(product);
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect($"/");
        }
    }
}
