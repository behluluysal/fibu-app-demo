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
    public class PaymentMapping : Profile
    {
        public PaymentMapping() 
        {
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, o => o.MapFrom(src => src.Price))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
                .ForMember(dest => dest.Method, o => o.MapFrom(src => src.Method))
                .ForMember(dest => dest.Date, o => o.MapFrom(src => src.Date))
                .ForMember(dest => dest.OfferId, o => o.MapFrom(src => src.OfferId));

            CreateMap<PaymentDto, Payment>()
               .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
               .ForMember(dest => dest.Price, o => o.MapFrom(src => src.Price))
               .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
               .ForMember(dest => dest.Method, o => o.MapFrom(src => src.Method))
               .ForMember(dest => dest.Date, o => o.MapFrom(src => src.Date))
               .ForMember(dest => dest.OfferId, o => o.MapFrom(src => src.OfferId));
        }
    }
}
