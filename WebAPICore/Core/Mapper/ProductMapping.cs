using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;


namespace Core.Mapper
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
                .ForMember(dest => dest.Image, o => o.MapFrom(src => src.Image));

            CreateMap<ProductDto, Product>()
              .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
              .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
              .ForMember(dest => dest.Image, o => o.MapFrom(src => src.Image));
        }
    }
}
