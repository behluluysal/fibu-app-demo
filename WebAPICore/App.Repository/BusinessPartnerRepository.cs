using App.Repository.ApiClient;
using Core.AutoMapperDtos;
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
    public class BusinessPartnerRepository : IBusinessPartnerRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public BusinessPartnerRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Requests

        public async Task<(int, IEnumerable<BusinessPartnerDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/businesspartners?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.businesspartners);
        }

        public async Task<BusinessPartnerDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/businesspartners/{id}");
            BusinessPartnerDto BusinessPartnerDtos = JsonSerializer.Deserialize<BusinessPartnerDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return BusinessPartnerDtos;
        }

        public async Task<BusinessPartnerDto> CreateAsync(BusinessPartnerDto supplier)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/businesspartners", supplier);
            BusinessPartnerDto BusinessPartnerDtos = JsonSerializer.Deserialize<BusinessPartnerDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return BusinessPartnerDtos;
        }

        public async Task UpdateAsync(int id, BusinessPartnerDto supplier)
        {
            await _webApiExecuter.InvokePut($"api/businesspartners/{id}", supplier);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/businesspartners/{id}");
        }

        #endregion


        #region Business Partner Responsible Person Requests

        public async Task<(int, BusinessPartnerWithResponsiblePersonDto)> GetWithResponsiblePeopleAsync(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/businesspartners/{id}/responsiblepeople?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseWithRPIndex parsedResult = JsonSerializer.Deserialize<ApiResponseWithRPIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.businesspartner);
        }

        #endregion

        #region Business Partner RequestedProduct Requests

        public async Task<(int, BusinessPartnerWithRequestsDto)> GetWithRequestsAsync(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/businesspartners/{id}/requests?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseWithRequestIndex parsedResult = JsonSerializer.Deserialize<ApiResponseWithRequestIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.businesspartner);
        }

        public async Task<BusinessPartnerWithRequestWithRequestedProductsDto> GetWithRequestWithRequestedProductsAsync(int id,int rid)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/businesspartners/{id}/requests/{rid}");
            BusinessPartnerWithRequestWithRequestedProductsDto BusinessPartnerDtos = JsonSerializer.Deserialize<BusinessPartnerWithRequestWithRequestedProductsDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return BusinessPartnerDtos;
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("businesspartners")]
            public IEnumerable<BusinessPartnerDto> businesspartners { get; set; }
        }

        public class ApiResponseWithRPIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("businesspartner")]
            public BusinessPartnerWithResponsiblePersonDto businesspartner { get; set; }
        }

        public class ApiResponseWithRequestIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("businesspartner")]
            public BusinessPartnerWithRequestsDto businesspartner { get; set; }
        }
    }
}
