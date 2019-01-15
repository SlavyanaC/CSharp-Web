namespace FunApp.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using FunApp.Services.Models.Contracts;

    public class ValidCategoryIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (ICategoriesService)validationContext
                .GetService(typeof(ICategoriesService));
            if (service != null && service.IsCategoryIdValid((int)value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid category id!");
        }
    }
}
