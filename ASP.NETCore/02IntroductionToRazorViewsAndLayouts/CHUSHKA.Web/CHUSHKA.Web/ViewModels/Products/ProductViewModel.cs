namespace CHUSHKA.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductViewModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        public ICollection<string> Types { get; set; }
    }
}
