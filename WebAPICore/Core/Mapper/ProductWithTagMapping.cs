using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;
using System.Linq;

namespace Core.Mapper
{
    public class ProductWithTagMapping : Profile
    {
        public ProductWithTagMapping()
        {
            CreateMap<Product, ProductWithTagDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
                .ForMember(dest => dest.Tags, o => o.MapFrom(src => src.ProductTags.Select(x=>x.Tag)))
                .ForMember(dest => dest.Image, o => o.MapFrom(src => src.Image));
        }
    }
}
