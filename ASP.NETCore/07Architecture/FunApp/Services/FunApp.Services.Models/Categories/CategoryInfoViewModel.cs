namespace FunApp.Services.Models.Categories
{
    using AutoMapper;
    using FunApp.Data.Models;
    using FunApp.Services.Mapping.Contracts;
    using System.Linq;

    public class CategoryInfoViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NameAndCount => $"{this.Name} ({this.CountOfAllJokes})";

        public int CountOfAllJokes { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Category, CategoryInfoViewModel>()
                .ForMember(opt => opt.CountOfAllJokes, dest => dest.MapFrom(c => c.Jokes.Count()));
        }
    }
}
