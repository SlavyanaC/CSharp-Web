namespace FunApp.Services.Models.Contracts
{
    using System.Collections.Generic;
    using FunApp.Services.Models.Categories;

    public interface ICategoriesService
    {
        IEnumerable<CategoryInfoViewModel> GetAll();

        bool IsCategoryIdValid(int categoryId);
    }
}
