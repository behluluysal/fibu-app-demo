using App.Repository.ApiClient;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
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
    public class RoleRepository : IRoleRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public RoleRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region Crud Operations

        public async Task<(int, IEnumerable<RoleDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/roles?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.roles);
        }

        public async Task<RoleDto> GetByIdAsync(string id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/roles/{id}");
            RoleDto roleDto = JsonSerializer.Deserialize<RoleDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return roleDto;
        }

        public async Task<RoleDto> CreateAsync(RoleDto role)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/roles", role);
            RoleDto roleDto = JsonSerializer.Deserialize<RoleDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return roleDto;
        }

        public async Task UpdateAsync(RoleDto role, string id)
        {
            await _webApiExecuter.InvokePut($"api/roles/{id}", role);
        }

        public async Task DeleteAsync(string id)
        {
            await _webApiExecuter.InvokeDelete($"api/roles/{id}");
        }

        #endregion


        #region Role Claim Operations

        public async Task<RoleWithClaimDto> GetRoleClaimsAsync(string id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/roles/{id}/claims");
            RoleWithClaimDto roleDto = JsonSerializer.Deserialize<RoleWithClaimDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return roleDto;
        }

        public async Task<RoleWithClaimDto> AssignClaimAsync(string id, ClaimDto claim)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>($"api/roles/{id}/claims", claim);
            RoleWithClaimDto roleDto = JsonSerializer.Deserialize<RoleWithClaimDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return roleDto;
        }

        //mustang idsi olmadığı için value ile siliyoruz
        public async Task WithdrawClaimAsync(string RoleId, string ClaimValue)
        {
            await _webApiExecuter.InvokeDelete($"api/roles/{RoleId}/claims/{ClaimValue}");
        }

        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("roles")]
            public IEnumerable<RoleDto> roles { get; set; }
        }
    }
}
