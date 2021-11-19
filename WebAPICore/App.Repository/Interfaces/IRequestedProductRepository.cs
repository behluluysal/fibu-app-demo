using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IRequestedProductRepository
    {
        Task<(int, IEnumerable<RequestedProductDto>)> GetAsync(QueryParams qp);
        Task<RequestedProductDto> CreateAsync(RequestedProductDto supplier);
        Task<RequestedProductDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, RequestedProductDto supplier);
        Task DeleteAsync(int id);
    }
}