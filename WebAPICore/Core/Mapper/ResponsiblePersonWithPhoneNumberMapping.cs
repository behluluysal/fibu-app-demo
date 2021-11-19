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
    public class ResponsiblePersonWithPhoneNumberMapping : Profile
    {
        public ResponsiblePersonWithPhoneNumberMapping()
        {
            CreateMap<ResponsiblePerson, ResponsiblePersonWithPhoneNumberDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, o => o.MapFrom(src => src.Position))
                .ForMember(dest => dest.PhoneNumbers, o => o.MapFrom(src => src.PhoneNumbers));
        }
    }
}
