namespace FunApp.Services.Models.Jokes
{
    using FunApp.Data.Models;
    using FunApp.Services.Mapping.Contracts;

    public class JokeDetailsViewModel : IMapFrom<Joke>
    {
        public string Content { get; set; }

        public string CategoryName { get; set; }
    }
}
