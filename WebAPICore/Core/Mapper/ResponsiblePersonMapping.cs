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
    public class ResponsiblePersonMapping : Profile
    {
        public ResponsiblePersonMapping()
        {
            CreateMap<ResponsiblePerson, ResponsiblePersonDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, o => o.MapFrom(src => src.Position))
                .ForMember(dest => dest.SupplierCompanyId, o => o.MapFrom(src => src.SupplierCompanyId));

            CreateMap<ResponsiblePersonDto, ResponsiblePerson>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Position, o => o.MapFrom(src => src.Position))
                .ForMember(dest => dest.SupplierCompanyId, o => o.MapFrom(src => src.SupplierCompanyId));
        }
    }
}
