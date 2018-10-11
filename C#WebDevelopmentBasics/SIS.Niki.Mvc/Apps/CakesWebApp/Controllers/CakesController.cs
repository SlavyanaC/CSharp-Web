﻿namespace CakesWebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Extensions;
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;

    public class CakesController : BaseController
    {
        [HttpGet("/cakes/add")]
        public IHttpResponse AddCakes()
        {
            return this.View("AddCakes");
        }

        [HttpPost("/cakes/add")]
        public IHttpResponse DoAddCakes()
        {
            var name = this.Request.FormData["name"].ToString().Trim().UrlDecode();
            var price = decimal.Parse(this.Request.FormData["price"].ToString().UrlDecode());
            var picture = this.Request.FormData["picture"].ToString().Trim().UrlDecode();

            // TODO: Validation

            var product = new Product
            {
                Name = name,
                Price = price,
                ImageUrl = picture
            };
            this.Db.Products.Add(product);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }

            // Redirect
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

            var viewBag = new Dictionary<string, string>
            {
                {"Name", product.Name},
                {"Price", product.Price.ToString(CultureInfo.InvariantCulture)},
                {"ImageUrl", product.ImageUrl}
            };
            return this.View("CakeById", viewBag);
        }
    }
}
