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
    public class SCRPPhoneNumberMapping : Profile
    {
        public SCRPPhoneNumberMapping()
        {
            CreateMap<SCRPPhoneNumber, SCRPPhoneNumberDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Gsm, o => o.MapFrom(src => src.Gsm))
                .ForMember(dest => dest.ResponsiblePersonId, o => o.MapFrom(src => src.ResponsiblePersonId));

            CreateMap<SCRPPhoneNumberDto, SCRPPhoneNumber>()
               .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
               .ForMember(dest => dest.Gsm, o => o.MapFrom(src => src.Gsm))
               .ForMember(dest => dest.ResponsiblePersonId, o => o.MapFrom(src => src.ResponsiblePersonId));
        }
    }
}
