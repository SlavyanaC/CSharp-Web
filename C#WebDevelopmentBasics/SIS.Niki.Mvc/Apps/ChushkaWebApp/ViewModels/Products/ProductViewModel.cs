namespace ChushkaWebApp.ViewModels.Products
{
    public class ProductViewModel
    {
        public string Id { get; set; } 

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get => this.Description.Length > 50 ? this.Description.Substring(0, 50) + "..." : this.Description;
            set {; }
        }

        public decimal Price { get; set; }
    }
}
