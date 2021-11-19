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
    public class ResponsiblePersonWithEmailMapping : Profile
    {
        public ResponsiblePersonWithEmailMapping()
        {
            CreateMap<ResponsiblePerson, ResponsiblePersonWithEmailDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, o => o.MapFrom(src => src.Position))
                .ForMember(dest => dest.Emails, o => o.MapFrom(src => src.Emails));
        }
    }
}
