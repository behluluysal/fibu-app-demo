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
    internal class BPRPPhoneNumberRepository : IBPRPPhoneNumberRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public BPRPPhoneNumberRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region Crud Operations

        public async Task<(int, IEnumerable<BPRPPhoneNumberDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bprpphonenumbers?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.bprpemails);
        }

        public async Task<BPRPPhoneNumberDto> CreateAsync(BPRPPhoneNumberDto scrpPhoneNumber)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/bprpphonenumbers", scrpPhoneNumber);
            BPRPPhoneNumberDto productDto = JsonSerializer.Deserialize<BPRPPhoneNumberDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }
        public async Task<BPRPPhoneNumberDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bprpphonenumbers/{id}");
            BPRPPhoneNumberDto productDto = JsonSerializer.Deserialize<BPRPPhoneNumberDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }

        public async Task UpdateAsync(BPRPPhoneNumberDto scrpPhoneNumber, int id)
        {
            await _webApiExecuter.InvokePut($"api/bprpphonenumbers/{id}", scrpPhoneNumber);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/bprpphonenumbers/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("bprpemails")]
            public IEnumerable<BPRPPhoneNumberDto> bprpemails { get; set; }
        }
    }
}
