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
    public class OfferMapping : Profile
    {
        public OfferMapping()
        {
            CreateMap<Offer, OfferDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, o => o.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Payment, o => o.MapFrom(src => src.Payment));

            CreateMap<OfferDto, Offer>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Amount, o => o.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Payment, o => o.MapFrom(src => src.Payment));
        }
    }
}
