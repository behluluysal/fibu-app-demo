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
    public class RequestedProductMapping : Profile
    {
        public RequestedProductMapping()
        {
            CreateMap<RequestedProduct, RequestedProductDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Quantity, o => o.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Deadline, o => o.MapFrom(src => src.Deadline))
                .ForMember(dest => dest.Product, o => o.MapFrom(src => src.Product))
                .ForMember(dest => dest.ProductId, o => o.MapFrom(src => src.ProductId));

            CreateMap<RequestedProductDto, RequestedProduct>()
               .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
               .ForMember(dest => dest.Quantity, o => o.MapFrom(src => src.Quantity))
               .ForMember(dest => dest.Deadline, o => o.MapFrom(src => src.Deadline))
               .ForMember(dest => dest.Product, o => o.MapFrom(src => src.Product))
               .ForMember(dest => dest.ProductId, o => o.MapFrom(src => src.ProductId));

        }
    }
}
