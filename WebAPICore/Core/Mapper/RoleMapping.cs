using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;


namespace Core.Mapper
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest=> dest.Id, o => o.MapFrom(src=> src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name));

            CreateMap<RoleDto, Role>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name));
        }
    }
}
