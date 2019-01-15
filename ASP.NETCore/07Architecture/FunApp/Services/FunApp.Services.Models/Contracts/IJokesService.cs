namespace FunApp.Services.Models.Contracts
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using FunApp.Services.Models.Home;
    using FunApp.Services.Models.Jokes;

    public interface IJokesService
    {
        IEnumerable<IndexJokeViewModel> GetRandomJokes(int count);

        int GetCount();

        Task<int> Create(int categoryId, string content);

        TViewModel GetJokeById<TViewModel>(int id);
    }
}
