using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IChatsScreenUseCases
    {
        Task<ChatDto> CreateChatAsync(ChatDto chat);
        Task DeleteChatAsync(string id);
        Task UpdateChatAsync(string id, ChatDto chat);
        Task<ChatDto> ViewChatByIdAsync(string id);
        Task<(int, IEnumerable<ChatDto>)> ViewChatsAsync(QueryParams qp);
    }
}