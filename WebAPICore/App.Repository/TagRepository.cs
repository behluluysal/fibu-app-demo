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
    public class TagRepository : ITagRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public TagRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<TagDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/tags?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.tags);
        }
        
        public async Task<TagDto> CreateAsync(TagDto tag)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/tags", tag);
            TagDto tagDto = JsonSerializer.Deserialize<TagDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return tagDto;
        }

        public async Task<TagDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/tags/{id}");
            TagDto tagDto = JsonSerializer.Deserialize<TagDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return tagDto;
        }


        public async Task UpdateAsync(int id, TagDto tag)
        {
            await _webApiExecuter.InvokePut($"api/tags/{id}", tag);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/tags/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("tags")]
            public IEnumerable<TagDto> tags { get; set; }
        }
    }
}
