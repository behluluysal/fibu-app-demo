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
    internal class SCRPEmailRepository : ISCRPEmailRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public SCRPEmailRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region Crud Operations

        public async Task<(int, IEnumerable<SCRPEmailDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/scrpemails?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.scrpemails);
        }

        public async Task<SCRPEmailDto> CreateAsync(SCRPEmailDto scrpemail)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/scrpemails", scrpemail);
            SCRPEmailDto scrpEmailDto = JsonSerializer.Deserialize<SCRPEmailDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return scrpEmailDto;
        }
        public async Task<SCRPEmailDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/scrpemails/{id}");
            SCRPEmailDto scrpEmailDto = JsonSerializer.Deserialize<SCRPEmailDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return scrpEmailDto;
        }

        public async Task UpdateAsync(SCRPEmailDto scrpemail, int id)
        {
            await _webApiExecuter.InvokePut($"api/scrpemails/{id}", scrpemail);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/scrpemails/{id}");
        }

        #endregion
        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("scrpemails")]
            public IEnumerable<SCRPEmailDto> scrpemails { get; set; }
        }
        
    }
}