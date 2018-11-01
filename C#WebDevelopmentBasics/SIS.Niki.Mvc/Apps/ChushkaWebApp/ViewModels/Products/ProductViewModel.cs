namespace ChushkaWebApp.ViewModels.Products
{
    public class ProductViewModel
    {
        private string description;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description
        {
            get => this.description;
            set
            {
                if (value.Length > 50)
                {
                    value = value.Substring(0, 50) + "...";
                }

                description = value;
            }
        }

        public decimal Price { get; set; }
    }
}
