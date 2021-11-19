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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public PaymentRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<PaymentDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/payments?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.payments);
        }

        public async Task<PaymentDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/payments/{id}");
            PaymentDto productDto = JsonSerializer.Deserialize<PaymentDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }

        public async Task<PaymentDto> CreateAsync(PaymentDto product)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/payments", product);
            PaymentDto productDto = JsonSerializer.Deserialize<PaymentDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }

        public async Task UpdateAsync(int id, PaymentDto product)
        {
            await _webApiExecuter.InvokePut($"api/payments/{id}", product);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/payments/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("payments")]
            public IEnumerable<PaymentDto> payments { get; set; }
        }

    }
}
