namespace CakesWebApp.ViewModels
{
    using System.Collections.Generic;

    public class SearchViewModel
    {
        public List<CakeViewModel> Cakes { get; set; }

        public string SearchText { get; set; }
    }
}
