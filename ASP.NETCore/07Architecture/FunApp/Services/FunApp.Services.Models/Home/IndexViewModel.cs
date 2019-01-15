namespace FunApp.Services.Models.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<IndexJokeViewModel> Jokes { get; set; }
    }
}
