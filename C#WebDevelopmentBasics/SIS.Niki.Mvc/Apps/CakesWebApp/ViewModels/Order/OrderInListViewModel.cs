namespace CakesWebApp.ViewModels.Order
{
    using System;

    public class OrderInListViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int NumberOfProducts { get; set; }

        public decimal SumOfProductPrices { get; set; }
    }
}
