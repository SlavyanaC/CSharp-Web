namespace CHUSHKA.Web
{
    using AutoMapper;
    using CHUSHKA.Models;
    using System.Globalization;
    using ViewModels.Orders;
    using ViewModels.Products;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.Ignore());

            CreateMap<Product, ProductDetailsViewModel>()
                .ReverseMap();

            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Client.UserName))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.OrderedOn, opt => opt.MapFrom(src => src.OrederedOn.ToString("hh:mm dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ReverseMap();
        }
    }
}