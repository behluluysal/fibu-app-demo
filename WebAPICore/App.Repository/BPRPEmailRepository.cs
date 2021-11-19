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
    public class BPRPEmailRepository : IBPRPEmailRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public BPRPEmailRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region Crud Operations

        public async Task<(int, IEnumerable<BPRPEmailDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bprpemails?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.bprpemails);

        }

        public async Task<BPRPEmailDto> CreateAsync(BPRPEmailDto bprpemail)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/bprpemails", bprpemail);
            BPRPEmailDto emailDto = JsonSerializer.Deserialize<BPRPEmailDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return emailDto;
        }
        public async Task<BPRPEmailDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/bprpemails/{id}");
            BPRPEmailDto emailDto = JsonSerializer.Deserialize<BPRPEmailDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return emailDto;
        }

        public async Task UpdateAsync(BPRPEmailDto bprpemail, int id)
        {
            await _webApiExecuter.InvokePut($"api/bprpemails/{id}", bprpemail);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/bprpemails/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("bprpemails")]
            public IEnumerable<BPRPEmailDto> bprpemails { get; set; }
        }
    }
}
