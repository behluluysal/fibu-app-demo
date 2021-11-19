using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IApplicationUserRepository
    {
        Task<(int, IEnumerable<ApplicationUserDto>)> GetAsync(QueryParams qp);
        Task<ApplicationUserDto> CreateAsync(CreateUserViewModel user);
        Task<ApplicationUserDto> GetByIdAsync(string id);
        Task UpdateAsync(CreateUserViewModel user, string id);
        Task DeleteAsync(string id);
        Task<ApplicationUserWithRoleDto> GetUserRoles(string userId);
        Task<ApplicationUserWithRoleDto> AssignRoleAsync(string userId, string roleId);
        Task WithdrawRoleFromUserAsync(string userId, string roleId);
    }
}