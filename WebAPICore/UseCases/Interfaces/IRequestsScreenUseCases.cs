using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IRequestsScreenUseCases
    {
        Task<(int, IEnumerable<RequestDto>)> ViewRequestsAsync(QueryParams qp);
        Task<RequestDto> CreateRequestAsync(RequestDto request);
        Task<RequestDto> ViewRequestByIdAsync(int id);
        Task UpdateRequestAsync(int id, RequestDto request);
        Task DeleteRequestAsync(int id);
        Task<RequestDto> ViewRequestByTokenAsync(string token);
    }
}