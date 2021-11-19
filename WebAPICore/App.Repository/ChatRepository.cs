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
    public class ChatRepository : IChatRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public ChatRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<ChatDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/chats?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.chats);
        }

        public async Task<ChatDto> GetByIdAsync(string id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/chats/{id}");
            ChatDto chatDto = JsonSerializer.Deserialize<ChatDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return chatDto;
        }

        public async Task<ChatDto> CreateAsync(ChatDto product)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/chats", product);
            ChatDto chatDto = JsonSerializer.Deserialize<ChatDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return chatDto;
        }

        public async Task UpdateAsync(string id, ChatDto product)
        {
            await _webApiExecuter.InvokePut($"api/chats/{id}", product);
        }

        public async Task DeleteAsync(string id)
        {
            await _webApiExecuter.InvokeDelete($"api/chats/{id}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("chats")]
            public IEnumerable<ChatDto> chats { get; set; }
        }

    }
}
