using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IRoleRepository
    {
        Task<(int, IEnumerable<RoleDto>)> GetAsync(QueryParams qp);
        Task<RoleDto> CreateAsync(RoleDto role);
        Task<RoleDto> GetByIdAsync(string id);
        Task UpdateAsync(RoleDto role, string id);
        Task DeleteAsync(string id);




        Task<RoleWithClaimDto> AssignClaimAsync(string id, ClaimDto claim);
        Task<RoleWithClaimDto> GetRoleClaimsAsync(string id);
        Task WithdrawClaimAsync(string RoleId, string ClaimValue);
    }
}