using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IBusinessPartnersScreenUseCases
    {
        Task<BusinessPartnerDto> CreateBusinessPartnerAsync(BusinessPartnerDto company);
        Task DeleteBusinessPartnerAsync(int id);
        Task UpdateBusinessPartnerAsync(int id, BusinessPartnerDto company);
        Task<BusinessPartnerDto> ViewBusinessPartnerByIdAsync(int id);
        Task<(int, BusinessPartnerWithRequestsDto)> ViewBusinessPartnerRequestsAsync(int id, QueryParams qp);
        Task<BusinessPartnerWithRequestWithRequestedProductsDto> ViewBusinessPartnerRequestWithRequestedProductsAsync(int id, int rid);
        Task<(int, BusinessPartnerWithResponsiblePersonDto)> ViewBusinessPartnerResponsiblePeoplesAsync(int id, QueryParams qp);
        Task<(int, IEnumerable<BusinessPartnerDto>)> ViewBusinessPartnersAsync(QueryParams qp);
    }
}