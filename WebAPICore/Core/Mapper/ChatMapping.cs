using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapper
{
    public class ChatMapping : Profile
    {
        public ChatMapping()
        {
            CreateMap<Chat, ChatDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedAt, o => o.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.IsNewMessage, o => o.MapFrom(src => src.IsNewMessage))
                .ForMember(dest => dest.SupplierCompanyId, o => o.MapFrom(src => src.SupplierCompanyId))
                .ForMember(dest => dest.SupplierCompany, o => o.MapFrom(src => src.SupplierCompany))
                .ForMember(dest => dest.RequestId, o => o.MapFrom(src => src.RequestId))
                .ForMember(dest => dest.Request, o => o.MapFrom(src => src.Request))
                .ForMember(dest => dest.Messages, o => o.MapFrom(src => src.Messages));

        }
    }
}
