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
    public class RequestWithRequestedProductMapping : Profile
    {
        public RequestWithRequestedProductMapping()
        {
            CreateMap<Request, RequestWithRequestedProductDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status))
                .ForMember(dest => dest.RequestedProducts, o => o.MapFrom(src => src.RequestedProducts));

            CreateMap<RequestWithRequestedProductDto, Request>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status));
        }
    }
}
