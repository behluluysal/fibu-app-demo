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
    public class SupplierCompanyWithTagMapping : Profile
    {
        public SupplierCompanyWithTagMapping()
        {
            CreateMap<SupplierCompany, SupplierCompanyWithTagDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone, o => o.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Adress, o => o.MapFrom(src => src.Adress))
                .ForMember(dest => dest.Tags, o => o.MapFrom(src => src.SupplierCompanyTags.Select(x=>x.Tag)));
        }
    }
}
