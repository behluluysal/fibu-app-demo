using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;


namespace Core.Mapper
{
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, o => o.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email));

        }
    }
}