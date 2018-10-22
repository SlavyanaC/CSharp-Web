namespace CakesWebApp.Controllers
{
    using System;
    using System.Linq;
    using SIS.HTTP.Responses.Contracts;
    using SIS.MvcFramework;
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
        public IHttpResponse DoAddCakes(DoAddCakesViewModel model)
        {
            // TODO: Validation
            var product = model.To<Product>();
            //var product = new Product
            //{
            //    Name = model.Name,
            //    Price = model.Price,
            //    ImageUrl = model.ImageUrl
            //};
            this.Db.Products.Add(product);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return this.Redirect($"/cakes/view?id={product.Id}");
        }

        // cakes/view?id=1
        [HttpGet("/cakes/view")]
        public IHttpResponse ById(int id)
        {
            //var id = int.Parse(this.Request.QueryData["id"].ToString());
            var product = this.Db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Cake not found.");
            }

            //var viewModel = new CakeViewModel()
            //{
            //    Name = product.Name,
            //    Price = product.Price,
            //    ImageUrl = product.ImageUrl,
            //};

            var viewModel = product.To<CakeViewModel>();

            return this.View("CakeById", viewModel);
        }

        [HttpGet("/cakes/search")]
        public IHttpResponse Search(string searchText)
        {
            var cakes = this.Db.Products.Where(p => p.Name.Contains(searchText))
                .Select(p => new CakeViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                }).ToList();

            var cakesViewModel = new SearchViewModel
            {
                Cakes = cakes,
                SearchText = searchText
            };

            return this.View("Search", cakesViewModel);
        }
    }
}
