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
    public class RequestWithRequestedProductWithTagsMapping : Profile
    {
        public RequestWithRequestedProductWithTagsMapping()
        {
            CreateMap<Request, RequestWithRequestedProductWithTagsDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status))
                .ForMember(dest => dest.RequestedProducts, o => o.MapFrom(src => src.RequestedProducts));

            CreateMap<RequestWithRequestedProductWithTagsDto, Request>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status));
        }
    }
}
