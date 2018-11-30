using System.Collections.Generic;

namespace FunApp.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<IndexJokeViewModel> Jokes { get; set; }
    }
}
