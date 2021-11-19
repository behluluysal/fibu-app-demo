using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IApplicationUsersScreenUseCases
    {
        Task<(int, IEnumerable<ApplicationUserDto>)> ViewUsersAsync(QueryParams qp);
        Task<ApplicationUserDto> CreateUserAsync(CreateUserViewModel user);
        Task<ApplicationUserDto> ViewUserByIdAsync(string id);
        Task UpdateUserAsync(CreateUserViewModel user, string id);
        Task DeleteUserAsync(string id);


        Task<ApplicationUserWithRoleDto> GetRolesAsync(string id);
        Task<ApplicationUserWithRoleDto> AssignRoleToUser(string userId, string roleId);
        Task WithdrawRoleFromUser(string userId, string roleId);
        
    }
}