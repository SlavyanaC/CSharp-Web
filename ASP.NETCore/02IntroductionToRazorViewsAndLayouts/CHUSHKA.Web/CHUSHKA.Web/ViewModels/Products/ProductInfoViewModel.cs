namespace CHUSHKA.Web.ViewModels.Products
{
    public class ProductInfoViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string DescriptionToDisplay => this.Description.Length > 50 ?
                    this.Description.Substring(0, 50) + "..." :
                    this.Description;
    }
}
