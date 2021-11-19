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
    public class OfferCreateMapping : Profile
    {
        public OfferCreateMapping()
        {
            CreateMap<OfferCreateDto, Offer>()
                .ForMember(dest => dest.Amount, o => o.MapFrom(src => src.Amount))
                .ForMember(dest => dest.RequestedProductId, o => o.MapFrom(src => src.RequestedProductId))
                .ForMember(dest => dest.SupplierCompanyId, o => o.MapFrom(src => src.SupplierCompanyId));
        }
    }
}