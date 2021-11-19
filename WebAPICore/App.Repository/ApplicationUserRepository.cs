using App.Repository.ApiClient;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public ApplicationUserRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        public async Task<(int, IEnumerable<ApplicationUserDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/applicationusers?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.applicationusers);
        }

        public async Task<ApplicationUserDto> GetByIdAsync(string id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/applicationusers/{id}");
            ApplicationUserDto applicationUser = JsonSerializer.Deserialize<ApplicationUserDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return applicationUser;
        }

        public async Task<ApplicationUserDto> CreateAsync(CreateUserViewModel user)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/applicationusers", user);
            ApplicationUserDto applicationUser = JsonSerializer.Deserialize<ApplicationUserDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return applicationUser;
        }

        public async Task UpdateAsync(CreateUserViewModel user, string id)
        {
            await _webApiExecuter.InvokePut($"api/applicationusers/{id}", user);
        }

        public async Task DeleteAsync(string id)
        {
            await _webApiExecuter.InvokeDelete($"api/applicationusers/{id}");
        }




        public async Task<ApplicationUserWithRoleDto> GetUserRoles(string userId)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/applicationusers/{userId}/roles");
            ApplicationUserWithRoleDto user = JsonSerializer.Deserialize<ApplicationUserWithRoleDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return user;
        }

        public async Task<ApplicationUserWithRoleDto> AssignRoleAsync(string userId, string roleId)
        {
            Role temp = null;
            var response = await _webApiExecuter.InvokePost<dynamic>($"api/applicationusers/{userId}/roles/{roleId}", temp);
            ApplicationUserWithRoleDto user = JsonSerializer.Deserialize<ApplicationUserWithRoleDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return user;
        }

        public async Task WithdrawRoleFromUserAsync(string userId, string roleId)
        {
            await _webApiExecuter.InvokeDelete($"api/applicationusers/{userId}/roles/{roleId}");
        }


        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("applicationusers")]
            public IEnumerable<ApplicationUserDto> applicationusers { get; set; }
        }
    }
}
