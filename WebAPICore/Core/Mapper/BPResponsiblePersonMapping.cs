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
    public class BPResponsiblePersonMapping : Profile
    {
        public BPResponsiblePersonMapping()
        {
            CreateMap<BPResponsiblePerson, BPResponsiblePersonDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.BusinessPartnerId, o => o.MapFrom(src => src.BusinessPartnerId))
                .ForMember(dest => dest.Position, o => o.MapFrom(src => src.Position));

            CreateMap<BPResponsiblePersonDto, BPResponsiblePerson>()
               .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
               .ForMember(dest => dest.BusinessPartnerId, o => o.MapFrom(src => src.BusinessPartnerId))
               .ForMember(dest => dest.Position, o => o.MapFrom(src => src.Position));
        }
    }
}
