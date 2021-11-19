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
    public class BusinessPartnerWithRequestsMapping : Profile
    {
        public BusinessPartnerWithRequestsMapping()
        {
            CreateMap<BusinessPartner, BusinessPartnerWithRequestsDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Gsm, o => o.MapFrom(src => src.Gsm))
                .ForMember(dest => dest.Phone, o => o.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.Adress, o => o.MapFrom(src => src.Adress))
                .ForMember(dest => dest.Requests, o => o.MapFrom(src => src.Requests));
        }
    }
}
