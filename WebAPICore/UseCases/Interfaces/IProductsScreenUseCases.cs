using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IProductsScreenUseCases
    {
        Task<(int, IEnumerable<ProductDto>)> ViewProductsAsync(QueryParams qp);
        Task<ProductDto> CreateProductAsync(ProductDto product);
        Task<ProductDto> ViewProductByIdAsync(int id);
        Task UpdateProductAsync(int id, ProductDto product);
        Task DeleteProductAsync(int id);


        Task<(int, ProductWithTagDto)> ViewProductTagsAsync(int pid, QueryParams qp);
        Task WithdrawTagFromProductAsync(int ProductId, int TagId);
        Task<IEnumerable<TagDto>> ViewUnassignedTagsAsync(int pid, QueryParams qp);
        Task AssignTagToProductAsync(int ProductId, int TagId);
    }
}