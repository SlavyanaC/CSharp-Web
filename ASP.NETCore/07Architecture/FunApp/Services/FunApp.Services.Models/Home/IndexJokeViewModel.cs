namespace FunApp.Services.Models.Home
{
    using FunApp.Data.Models;
    using FunApp.Services.Mapping.Contracts;

    public class IndexJokeViewModel : IMapFrom<Joke>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string CategoryName { get; set; }
    }
}
