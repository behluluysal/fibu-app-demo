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
    public class SupplierCompanyWithoutContactMapping : Profile
    {
        public SupplierCompanyWithoutContactMapping()
        {
            CreateMap<SupplierCompany, SupplierCompanyWithoutContactDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name));

            CreateMap<SupplierCompanyWithoutContactDto, SupplierCompany>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name));
        }
    }
}
