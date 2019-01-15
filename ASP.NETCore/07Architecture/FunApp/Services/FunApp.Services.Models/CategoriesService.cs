namespace FunApp.Services.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using FunApp.Data.Models;
    using FunApp.Data.Common;
    using FunApp.Services.Models.Categories;
    using FunApp.Services.Models.Contracts;
    using FunApp.Services.Mapping;

    public class CategoriesService : ICategoriesService
    {
        private readonly IRepository<Category> categoriesRepository;

        public CategoriesService(IRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<CategoryInfoViewModel> GetAll()
        {
            var categories = this.categoriesRepository.All()
                .To<CategoryInfoViewModel>()
                .ToArray();

            return categories;
        }

        public bool IsCategoryIdValid(int categoryId) => this.categoriesRepository.All().Any(c => c.Id == categoryId);
    }
}
