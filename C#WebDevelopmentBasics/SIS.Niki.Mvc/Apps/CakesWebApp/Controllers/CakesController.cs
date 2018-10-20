﻿namespace CakesWebApp.Controllers
{
    using System;
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using SIS.MvcFramework.Loggers.Contracts;
    using Models;
    using ViewModels;

    public class CakesController : BaseController
    {
        private readonly ILogger logger;

        public CakesController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet("/cakes/add")]
        public IHttpResponse AddCakes()
        {
            return this.View("AddCakes");
        }

        [HttpPost("/cakes/add")]
        public IHttpResponse DoAddCakes(DoAddCakesInputModel model)
        {
            // TODO: Validation

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                ImageUrl = model.Picture
            };
            this.Db.Products.Add(product);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect("/");
        }

        // cakes/view?id=1
        [HttpGet("/cakes/view")]
        public IHttpResponse ById()
        {
            var id = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.Db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Cake not found.");
            }

            var viewModel = new ByIdInputModel()
            {
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
            };

            return this.View("CakeById", viewModel);
        }
    }
}