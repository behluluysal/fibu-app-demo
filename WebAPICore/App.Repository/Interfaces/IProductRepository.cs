using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IProductRepository
    {
        Task<(int, IEnumerable<ProductDto>)> GetAsync(QueryParams qp);
        Task<ProductDto> CreateAsync(ProductDto Product);
        Task<ProductDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, ProductDto Product);
        Task DeleteAsync(int id);


        Task<(int, ProductWithTagDto)> GetProductTagsById(int id, QueryParams qp);
        Task WithdrawTagAsync(int ProductId, int TagId);
        Task AssignTagAsync(int ProductId, int TagId);
    }
}