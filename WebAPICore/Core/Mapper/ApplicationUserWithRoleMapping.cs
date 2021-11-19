using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;


namespace Core.Mapper
{
    public class ApplicationUserWithRoleMapping : Profile
    {
        public ApplicationUserWithRoleMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserWithRoleDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, o => o.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom((src, dst, _, context) => context.Options.Items["Roles"]));
        }
    }
}
