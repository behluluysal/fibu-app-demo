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
    public class BPRPPhoneNumberMapping : Profile
    {
        public BPRPPhoneNumberMapping()
        {
            CreateMap<BPRPPhoneNumber, BPRPPhoneNumberDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Gsm, o => o.MapFrom(src => src.Gsm))
                .ForMember(dest => dest.ResponsiblePersonId, o => o.MapFrom(src => src.BPResponsiblePersonId));

            CreateMap<BPRPPhoneNumberDto, BPRPPhoneNumber>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Gsm, o => o.MapFrom(src => src.Gsm))
                .ForMember(dest => dest.BPResponsiblePersonId, o => o.MapFrom(src => src.ResponsiblePersonId));
        }
    }
}
