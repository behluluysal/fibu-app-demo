using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IBusinessPartnerRepository
    {
        Task<(int, IEnumerable<BusinessPartnerDto>)> GetAsync(QueryParams qp);
        Task<BusinessPartnerDto> CreateAsync(BusinessPartnerDto supplier);
        Task<BusinessPartnerDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, BusinessPartnerDto supplier);
        Task DeleteAsync(int id);

        Task<(int, BusinessPartnerWithResponsiblePersonDto)> GetWithResponsiblePeopleAsync(int id, QueryParams qp);


        Task<(int, BusinessPartnerWithRequestsDto)> GetWithRequestsAsync(int id, QueryParams qp);
        Task<BusinessPartnerWithRequestWithRequestedProductsDto> GetWithRequestWithRequestedProductsAsync(int id, int rid);
       
    }
}