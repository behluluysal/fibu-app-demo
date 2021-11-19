using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IOfferScreenUseCases
    {
        Task<OfferWithSupplierCompanyDto> CreateOfferAsync(OfferCreateDto company);
        Task DeleteOfferAsync(int id);
        Task<OfferWithSupplierCompanyDto> GetApprovedOffer(QueryParams qp, int requestedProductId);
        Task UpdateOfferAsync(int id, OfferWithSupplierCompanyDto company);
        Task<OfferWithSupplierCompanyDto> ViewOfferByIdAsync(int id);
        Task<(int, IEnumerable<OfferWithSupplierCompanyDto>)> ViewOffersAsync(QueryParams qp);
        Task<(int, IEnumerable<OfferWithSupplierCompanyDto>)> ViewOffersOfRequestedProductAsync(QueryParams qp, int requestedProductId);
    }
}