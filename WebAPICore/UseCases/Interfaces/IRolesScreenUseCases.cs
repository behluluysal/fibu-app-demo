using App.Repository;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IRolesScreenUseCases
    {
        Task<(int, IEnumerable<RoleDto>)> ViewRolesAsync(QueryParams qp);
        Task<RoleDto> CreateRoleAsync(RoleDto role);
        Task<RoleDto> ViewRoleByIdAsync(string id);
        Task UpdateRoleAsync(string id, RoleDto role);
        Task DeleteRoleAsync(string id);


        Task<RoleWithClaimDto> GetClaimsAsync(string id);
        Task<RoleWithClaimDto> AssignClaimToRoleAsync(string id, ClaimDto claim);
        Task WithdrawClaimFromRoleAsync(string RoleId, string ClaimValue);
    }
}