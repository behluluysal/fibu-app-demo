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
    public class OfferRepository : IOfferRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public OfferRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<OfferWithSupplierCompanyDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/offers?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.offers);
        }

        public async Task<OfferWithSupplierCompanyDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/offers/{id}");
            OfferWithSupplierCompanyDto productDto = JsonSerializer.Deserialize<OfferWithSupplierCompanyDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }

        public async Task<OfferWithSupplierCompanyDto> CreateAsync(OfferCreateDto product)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/offers", product);
            OfferWithSupplierCompanyDto productDto = JsonSerializer.Deserialize<OfferWithSupplierCompanyDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }

        public async Task UpdateAsync(int id, OfferWithSupplierCompanyDto product)
        {
            await _webApiExecuter.InvokePut($"api/offers/{id}", product);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/offers/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("offers")]
            public IEnumerable<OfferWithSupplierCompanyDto> offers { get; set; }
        }
    }
}
