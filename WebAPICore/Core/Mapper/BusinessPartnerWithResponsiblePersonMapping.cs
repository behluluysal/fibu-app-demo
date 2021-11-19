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
    public class BusinessPartnerWithResponsiblePersonMapping : Profile
    {
        public BusinessPartnerWithResponsiblePersonMapping()
        {
            CreateMap<BusinessPartner, BusinessPartnerWithResponsiblePersonDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone, o => o.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Adress, o => o.MapFrom(src => src.Adress))
                .ForMember(dest => dest.ResponsiblePeople, o => o.MapFrom(src => src.BPResponsiblePeople));
        }
    }
}
