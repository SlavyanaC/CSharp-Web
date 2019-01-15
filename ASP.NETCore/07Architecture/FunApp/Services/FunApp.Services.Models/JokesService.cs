namespace FunApp.Services.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FunApp.Data.Models;
    using FunApp.Data.Common;
    using FunApp.Services.Models.Home;
    using FunApp.Services.Models.Jokes;
    using FunApp.Services.Models.Contracts;
    using FunApp.Services.Mapping;

    public class JokesService : IJokesService
    {
        private readonly IRepository<Joke> jokesRepository;
        private readonly IRepository<Category> categoryRepository;

        public JokesService(IRepository<Joke> jokesRepository, IRepository<Category> categoryRepository)
        {
            this.jokesRepository = jokesRepository;
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<IndexJokeViewModel> GetRandomJokes(int count)
        {
            var jokes = this.jokesRepository.All()
                .OrderBy(j => Guid.NewGuid())
                .To<IndexJokeViewModel>()
                .Take(count)
                .ToArray();

            return jokes;
        }

        public int GetCount() => this.jokesRepository.All().Count();

        public async Task<int> Create(int categoryId, string content)
        {
            var joke = new Joke
            {
                CategoryId = categoryId,
                Content = content,
            };

            await this.jokesRepository.AddAsync(joke);
            await this.jokesRepository.SaveChangesAsync();

            return joke.Id;
        }

        public TViewModel GetJokeById<TViewModel>(int id)
        {
            var joke = this.jokesRepository.All()
                .Where(j => j.Id == id)
                .To<JokeDetailsViewModel>()
                .SingleOrDefault();

            throw new NotImplementedException();
            //return joke;
        }
    }
}
