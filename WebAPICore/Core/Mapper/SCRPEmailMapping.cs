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
    public class SCRPEmailMapping : Profile
    {
        public SCRPEmailMapping()
        {
            CreateMap<SCRPEmail, SCRPEmailDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.ResponsiblePersonId, o => o.MapFrom(src => src.ResponsiblePersonId));

            CreateMap<SCRPEmailDto, SCRPEmail>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.ResponsiblePersonId, o => o.MapFrom(src => src.ResponsiblePersonId));

        }
    }
}
