using App.Repository;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static App.Repository.RoleRepository;

namespace UseCases
{
    public class RolesScreenUseCases : IRolesScreenUseCases
    {
        private readonly IRoleRepository _roleRepository;

        public RolesScreenUseCases(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        #region Crud Operations

        public async Task<(int, IEnumerable<RoleDto>)> ViewRolesAsync(QueryParams qp)
        {
            return await _roleRepository.GetAsync(qp);
        }
        public async Task<RoleDto> CreateRoleAsync(RoleDto role)
        {
            return await _roleRepository.CreateAsync(role);
        }

        public async Task<RoleDto> ViewRoleByIdAsync(string id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task UpdateRoleAsync(string id, RoleDto role)
        {
            await _roleRepository.UpdateAsync(role, id);
        }

        public async Task DeleteRoleAsync(string id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        #endregion

        #region Role Claim Operations

        public async Task<RoleWithClaimDto> GetClaimsAsync(string id)
        {
            return await _roleRepository.GetRoleClaimsAsync(id);
        }

        public async Task<RoleWithClaimDto> AssignClaimToRoleAsync(string id, ClaimDto claim)
        {
            return await _roleRepository.AssignClaimAsync(id, claim);
        }

        public async Task WithdrawClaimFromRoleAsync(string RoleId, string ClaimValue)
        {
            await _roleRepository.WithdrawClaimAsync(RoleId, ClaimValue);
        }

        #endregion
    }
}
