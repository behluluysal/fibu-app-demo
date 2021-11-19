using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;


namespace Core.Mapper
{
    public class ProductCreateMapping : Profile
    {
        public ProductCreateMapping()
        {
            CreateMap<Product, ProductCreateDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description));
        }
    }
}
