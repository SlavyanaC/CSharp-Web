namespace FunApp.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using FunApp.Services.Models.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using FunApp.Services.Models.Jokes;
    using FunApp.Web.Models.Jokes;

    public class JokesController : BaseController
    {
        private readonly IJokesService jokesService;
        private readonly ICategoriesService categoriesService;

        public JokesController(IJokesService jokesService, ICategoriesService categoriesService)
        {
            this.jokesService = jokesService;
            this.categoriesService = categoriesService;
        }

        [Authorize]
        public IActionResult Create()
        {
            this.ViewData["Categories"] = this.categoriesService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.NameAndCount,
                })
                .OrderBy(c => c.Text);

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateJokeInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var id = await this.jokesService.Create(inputModel.CategoryId, inputModel.Content);
            return this.RedirectToAction("Details", new { id = id });
        }

        public IActionResult Details(int id)
        {
            var joke = this.jokesService.GetJokeById<JokeDetailsViewModel>(id);
            return this.View(joke);
        }
    }
}
