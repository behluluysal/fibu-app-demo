using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IOfferRepository
    {
        Task<OfferWithSupplierCompanyDto> CreateAsync(OfferCreateDto product);
        Task DeleteAsync(int id);
        Task<(int, IEnumerable<OfferWithSupplierCompanyDto>)> GetAsync(QueryParams qp);
        Task<OfferWithSupplierCompanyDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, OfferWithSupplierCompanyDto product);
    }
}