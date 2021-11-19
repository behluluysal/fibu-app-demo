using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface ISupplierCompaniesScreenUseCases
    {
        Task<(int, IEnumerable<SupplierCompanyDto>)> ViewSupplierCompaniesAsync(QueryParams qp);
        Task<SupplierCompanyDto> CreateSupplierCompanyAsync(SupplierCompanyDto company);
        Task<SupplierCompanyDto> ViewSupplierCompanyByIdAsync(int id);
        Task UpdateSupplierCompanyAsync(int id, SupplierCompanyDto company);
        Task DeleteSupplierCompanyAsync(int id);


        Task<SupplierCompanyWithTagDto> ViewSupplierCompanyTagsAsync(int id);
        Task WithdrawTagFromSupplierCompanyAsync(int CompanyId, int TagId);
        Task AssignTagToSupplierCompanyAsync(int CompanyId, int TagId);


        Task<SupplierCompanyWithResponsiblePersonDto> ViewSupplierCompanyResponsiblePeoplesAsync(int id, QueryParams qp);
        Task<SupplierCompanyDto> ViewSupplierCompanyByTokenAsync(string token);
    }
}