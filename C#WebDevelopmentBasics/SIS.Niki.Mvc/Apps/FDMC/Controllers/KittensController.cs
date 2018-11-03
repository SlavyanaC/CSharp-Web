namespace FDMCWebApp.Controllers
{
    using System;
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework.Attributes;
    using Models;
    using Models.Enums;
    using ViewModels.Kittens;

    public class KittensController : BaseController
    {
        [Authorize]
        public IHttpResponse All()
        {
            var kittens = this.DbContext.Kittens.Select(k => new KittenViewModel
            {
                Name = k.Name,
                Age = k.Age,
                Breed = k.Breed.ToString(),
            }).ToArray();


            return this.View(kittens);
        }

        [Authorize]
        public IHttpResponse Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Add(KittenViewModel model)
        {
            if (!Enum.TryParse(model.Breed.Replace("-", "_").Replace(" ", "_").Trim(), true, out Breed breed))
            {
                return this.BadRequestErrorWithView($"Invalid breed");
            }

            var kitten = new Kitten
            {
                Name = model.Name,
                Age = model.Age,
                Breed = breed,
            };

            this.DbContext.Kittens.Add(kitten);
            this.DbContext.SaveChanges();

            return this.Redirect("/kittens/all");
        }
    }
}