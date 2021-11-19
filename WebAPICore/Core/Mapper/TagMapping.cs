using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;


namespace Core.Mapper
{
    public class TagMapping : Profile
    {
        public TagMapping()
        {
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name));

            CreateMap<TagDto, Tag>()
               .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name));
        }
    }
}
