using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapper
{
    public class BPRPEmailMapping : Profile
    {
        public BPRPEmailMapping()
        {
            CreateMap<BPRPEmail, BPRPEmailDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.CanLogin, o => o.MapFrom(src => src.CanLogin))
                .ForMember(dest => dest.ResponsiblePersonId, o => o.MapFrom(src => src.BPResponsiblePersonId));

            CreateMap<BPRPEmailDto, BPRPEmail>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.CanLogin, o => o.MapFrom(src => src.CanLogin))
                .ForMember(dest => dest.BPResponsiblePersonId, o => o.MapFrom(src => src.ResponsiblePersonId));
        }
    }
}
