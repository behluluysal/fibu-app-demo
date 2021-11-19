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
    public class RequestMapping : Profile
    {
        public RequestMapping()
        {
            CreateMap<Request, RequestDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status))
                .ForMember(dest => dest.BusinessPartnerId, o => o.MapFrom(src => src.BusinessPartnerId))
                .ForMember(dest => dest.BusinessPartner, o => o.MapFrom(src => src.BusinessPartner))
                .ForMember(dest => dest.No, o => o.MapFrom(src => src.No));

            CreateMap<RequestDto, Request>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status))
                .ForMember(dest => dest.BusinessPartnerId, o => o.MapFrom(src => src.BusinessPartnerId))
                .ForMember(dest => dest.BusinessPartner, o => o.MapFrom(src => src.BusinessPartner))
                .ForMember(dest => dest.No, o => o.MapFrom(src => src.No));
        }
    }
}
