using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface ISupplierCompanyRepository
    {
        Task<(int, IEnumerable<SupplierCompanyDto>)> GetAsync(QueryParams qp);
        Task<SupplierCompanyDto> CreateAsync(SupplierCompanyDto supplier);
        Task<SupplierCompanyDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, SupplierCompanyDto supplier);
        Task DeleteAsync(int id);


        Task<SupplierCompanyWithTagDto> GetWithTagAsync(int id);
        Task AssignTagAsync(int CompanyId, int TagId);
        Task WithdrawTagAsync(int CompanyId, int TagId);



        Task<SupplierCompanyWithResponsiblePersonDto> GetWithResponsiblePeopleAsync(int id, QueryParams qp);
        
       
        
    }
}