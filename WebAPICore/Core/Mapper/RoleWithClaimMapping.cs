using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;


namespace Core.Mapper
{
    public class RoleWithClaimMapping : Profile
    {
        public RoleWithClaimMapping()
        {
            CreateMap<Role, RoleWithClaimDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Claims, opt => opt.MapFrom((src, dst, _, context) => context.Options.Items["Claims"]));
        }
    }
}
