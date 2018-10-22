namespace CakesWebApp.ViewModels
{
    using System.Collections.Generic;

    public class OrderViewModel
    {
        public int Id { get; set; }

        public bool IsShoppingCart { get; set; }

        public IEnumerable<CakeViewModel> Cakes { get; set; }
    }
}
