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
    public class RequestedProductRepository : IRequestedProductRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public RequestedProductRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }


        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<RequestedProductDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/requestedproducts?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.requestedproducts);
        }

        public async Task<RequestedProductDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/requestedproducts/{id}");
            RequestedProductDto requestedProductDtos = JsonSerializer.Deserialize<RequestedProductDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return requestedProductDtos;
        }

        public async Task<RequestedProductDto> CreateAsync(RequestedProductDto supplier)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/requestedproducts", supplier);
            RequestedProductDto requestedProductDtos = JsonSerializer.Deserialize<RequestedProductDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return requestedProductDtos;
        }

        public async Task UpdateAsync(int id, RequestedProductDto supplier)
        {
            await _webApiExecuter.InvokePut($"api/requestedproducts/{id}", supplier);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/requestedproducts/{id}");
        }

        #endregion
        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("requestedproducts")]
            public IEnumerable<RequestedProductDto> requestedproducts { get; set; }
        }
        
    }
}
