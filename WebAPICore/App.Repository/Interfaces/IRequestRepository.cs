using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IRequestRepository
    {
        Task<(int, IEnumerable<RequestDto>)> GetAsync(QueryParams qp);
        Task<RequestDto> CreateAsync(RequestDto request);
        Task<RequestDto> GetByIdAsync(int id);
        Task UpdateAsync(RequestDto request, int id);
        Task DeleteAsync(int id);
    }
}