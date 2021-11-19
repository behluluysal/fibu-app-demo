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
    internal class SCRPPhoneNumberRepository : ISCRPPhoneNumberRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public SCRPPhoneNumberRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region Crud Operations

        public async Task<(int, IEnumerable<SCRPPhoneNumberDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/scrpphonenumbers?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.scrpphonenumbers);
        }

        public async Task<SCRPPhoneNumberDto> CreateAsync(SCRPPhoneNumberDto scrpPhoneNumber)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/scrpphonenumbers", scrpPhoneNumber);
            SCRPPhoneNumberDto scrpPhoneNumberDto = JsonSerializer.Deserialize<SCRPPhoneNumberDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return scrpPhoneNumberDto;
        }
        public async Task<SCRPPhoneNumberDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/scrpphonenumbers/{id}");
            SCRPPhoneNumberDto scrpPhoneNumberDto = JsonSerializer.Deserialize<SCRPPhoneNumberDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return scrpPhoneNumberDto;
        }

        public async Task UpdateAsync(SCRPPhoneNumberDto scrpPhoneNumber, int id)
        {
            await _webApiExecuter.InvokePut($"api/scrpphonenumbers/{id}", scrpPhoneNumber);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/scrpphonenumbers/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("scrpphonenumbers")]
            public IEnumerable<SCRPPhoneNumberDto> scrpphonenumbers { get; set; }
        }
    }
}
