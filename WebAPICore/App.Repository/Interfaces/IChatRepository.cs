using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IChatRepository
    {
        Task<ChatDto> CreateAsync(ChatDto product);
        Task DeleteAsync(string id);
        Task<(int, IEnumerable<ChatDto>)> GetAsync(QueryParams qp);
        Task<ChatDto> GetByIdAsync(string id);
        Task UpdateAsync(string id, ChatDto product);
    }
}