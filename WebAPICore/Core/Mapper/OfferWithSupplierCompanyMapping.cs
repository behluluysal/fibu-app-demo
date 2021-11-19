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
    public class OfferWithSupplierCompanyMapping : Profile
    {
        public OfferWithSupplierCompanyMapping()
        {
            CreateMap<Offer, OfferWithSupplierCompanyDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, o => o.MapFrom(src => src.Amount))
                .ForMember(dest => dest.SupplierCompanyId, o => o.MapFrom(src => src.SupplierCompanyId))
                .ForMember(dest => dest.RequestedProductId, o => o.MapFrom(src => src.RequestedProductId))
                .ForMember(dest => dest.PaymentId, o => o.MapFrom(src => src.PaymentId))
                .ForMember(dest => dest.RequestedProduct, o => o.MapFrom(src => src.RequestedProduct))
                .ForMember(dest => dest.SupplierCompany, o => o.MapFrom(src => src.SupplierCompany))
                .ForMember(dest => dest.Payment, o => o.MapFrom(src => src.Payment))
                .ForMember(dest => dest.isConfirmedOffer, o => o.MapFrom(src => src.isConfirmedOffer));
        }
    }
}
