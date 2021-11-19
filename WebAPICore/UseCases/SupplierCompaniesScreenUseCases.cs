using App.Repository;
using Core.AutoMapperDtos;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UseCases
{
    public class SupplierCompaniesScreenUseCases : ISupplierCompaniesScreenUseCases
    {
        private readonly ISupplierCompanyRepository _supplierCompanyRepository;
        public SupplierCompaniesScreenUseCases(ISupplierCompanyRepository supplierCompanyRepository)
        {
            _supplierCompanyRepository = supplierCompanyRepository;
        }



        #region CRUD Methods

        public async Task<(int, IEnumerable<SupplierCompanyDto>)> ViewSupplierCompaniesAsync(QueryParams qp)
        {
            return await _supplierCompanyRepository.GetAsync(qp);
        }

        public async Task<SupplierCompanyDto> CreateSupplierCompanyAsync(SupplierCompanyDto company)
        {
            return await _supplierCompanyRepository.CreateAsync(company);
        }

        public async Task<SupplierCompanyDto> ViewSupplierCompanyByIdAsync(int id)
        {
            return await _supplierCompanyRepository.GetByIdAsync(id);
        }
        public async Task<SupplierCompanyDto> ViewSupplierCompanyByTokenAsync(string token)
        {
            var response = await _supplierCompanyRepository.GetAsync(new QueryParams() {Filter=$"(Token == \"{token}\")" });
            IEnumerable<SupplierCompanyDto> supplierCompanies = response.Item2;
            return supplierCompanies.ElementAtOrDefault(0);
        }

        public async Task UpdateSupplierCompanyAsync(int id, SupplierCompanyDto company)
        {
            await _supplierCompanyRepository.UpdateAsync(id, company);
        }

        public async Task DeleteSupplierCompanyAsync(int id)
        {
            await _supplierCompanyRepository.DeleteAsync(id);
        }

        #endregion

        #region Supplier Company Tag Methods

        public async Task<SupplierCompanyWithTagDto> ViewSupplierCompanyTagsAsync(int id)
        {
            return await _supplierCompanyRepository.GetWithTagAsync(id);
        }
        public async Task AssignTagToSupplierCompanyAsync(int CompanyId, int TagId)
        {
            await _supplierCompanyRepository.AssignTagAsync(CompanyId, TagId);
        }

        public async Task WithdrawTagFromSupplierCompanyAsync(int CompanyId, int TagId)
        {
            await _supplierCompanyRepository.WithdrawTagAsync(CompanyId, TagId);
        }

        #endregion

        #region Supplier Company ResponsiblePerson Methods

        public async Task<SupplierCompanyWithResponsiblePersonDto> ViewSupplierCompanyResponsiblePeoplesAsync(int id, QueryParams qp)
        {
            return await _supplierCompanyRepository.GetWithResponsiblePeopleAsync(id, qp);
        }

        #endregion
    }
}
