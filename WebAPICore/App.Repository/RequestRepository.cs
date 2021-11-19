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
    public class RequestRepository : IRequestRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public RequestRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region Crud Operations

        public async Task<(int, IEnumerable<RequestDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/requests?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.requests);
        }

        public async Task<RequestDto> CreateAsync(RequestDto request)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/requests", request);
            RequestDto requestDto = JsonSerializer.Deserialize<RequestDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return requestDto;
        }
        public async Task<RequestDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/requests/{id}");
            RequestDto requestDto = JsonSerializer.Deserialize<RequestDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return requestDto;
        }

        public async Task UpdateAsync(RequestDto request, int id)
        {
            await _webApiExecuter.InvokePut($"api/requests/{id}", request);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/requests/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("requests")]
            public IEnumerable<RequestDto> requests { get; set; }
        }
    }
}
