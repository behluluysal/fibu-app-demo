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
    public class MessageMapping : Profile
    {
        public MessageMapping()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.AdminMessage, o => o.MapFrom(src => src.AdminMessage))
                .ForMember(dest => dest.MessageText, o => o.MapFrom(src => src.MessageText))
                .ForMember(dest => dest.SentTime, o => o.MapFrom(src => src.SentTime));

        }
    }
}
