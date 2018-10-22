namespace CakesWebApp.ViewModels.Order
{
    using System.Collections.Generic;
    using Cake;

    public class OrderViewModel
    {
        public int Id { get; set; }

        public bool IsShoppingCart { get; set; }

        public IEnumerable<CakeViewModel> Products { get; set; }
    }
}
