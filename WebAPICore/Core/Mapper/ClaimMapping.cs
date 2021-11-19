using AutoMapper;
using Core.AutoMapperDtos;
using System.Security.Claims;


namespace Core.Mapper
{
    public class ClaimMapping : Profile
    {
        public ClaimMapping()
        {
            CreateMap<Claim, ClaimDto>()
                .ForMember(dest => dest.Issuer, o => o.MapFrom(src => src.Issuer))
                .ForMember(dest => dest.Type, o => o.MapFrom(src => src.Type))
                .ForMember(dest => dest.Value, o => o.MapFrom(src => src.Value));
        }
    }
}
