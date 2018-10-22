namespace CakesWebApp.ViewModels
{
    using System.Collections.Generic;

    public class SearchViewModel
    {
        public List<CakeByIdViewModel> Cakes { get; set; }

        public string SearchText { get; set; }
    }
}
