using App.Repository.ApiClient;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Repository
{
    public class SupplierCompanyRepository : ISupplierCompanyRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public SupplierCompanyRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<SupplierCompanyDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/suppliercompanies?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.companies);
        }

        public async Task<SupplierCompanyDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/suppliercompanies/{id}");
            SupplierCompanyDto supplierCompanyDtos = JsonSerializer.Deserialize<SupplierCompanyDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return supplierCompanyDtos;
        }

        public async Task<SupplierCompanyDto> CreateAsync(SupplierCompanyDto supplier)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/suppliercompanies", supplier);
            SupplierCompanyDto supplierCompanyDtos = JsonSerializer.Deserialize<SupplierCompanyDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return supplierCompanyDtos;
        }

        public async Task UpdateAsync(int id, SupplierCompanyDto supplier)
        {
            await _webApiExecuter.InvokePut($"api/suppliercompanies/{id}", supplier);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/suppliercompanies/{id}");
        }

        #endregion

        #region SupplierCompany Tag Methods
        public async Task<SupplierCompanyWithTagDto> GetWithTagAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/suppliercompanies/{id}/tags");
            SupplierCompanyWithTagDto supplierCompanyDtos = JsonSerializer.Deserialize<SupplierCompanyWithTagDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return supplierCompanyDtos;
        }

        public async Task AssignTagAsync(int CompanyId, int TagId)
        {
            Tag temp = null;
            await _webApiExecuter.InvokePost($"api/suppliercompanies/{CompanyId}/tags/{TagId}", temp);
        }
        public async Task WithdrawTagAsync(int CompanyId, int TagId)
        {
            await _webApiExecuter.InvokeDelete($"api/suppliercompanies/{CompanyId}/tags/{TagId}");
        }
        #endregion

        #region Supplier Company Responsible Person Requests

        public async Task<SupplierCompanyWithResponsiblePersonDto> GetWithResponsiblePeopleAsync(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/suppliercompanies/{id}/responsiblepeople?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            SupplierCompanyWithResponsiblePersonDto supplierCompanyDtos = JsonSerializer.Deserialize<SupplierCompanyWithResponsiblePersonDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return supplierCompanyDtos;
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("companies")]
            public IEnumerable<SupplierCompanyDto> companies { get; set; }
        }
    }
}
