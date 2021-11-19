using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IRequestedProductsScreenUseCases
    {
        Task<(int, IEnumerable<RequestedProductDto>)> ViewRequestedProductsAsync(QueryParams qp);
        Task<RequestedProductDto> CreateRequestedProductAsync(RequestedProductDto company);
        Task<RequestedProductDto> ViewRequestedProductByIdAsync(int id);
        Task UpdateRequestedProductAsync(int id, RequestedProductDto company);
        Task DeleteRequestedProductAsync(int id);
    }
}