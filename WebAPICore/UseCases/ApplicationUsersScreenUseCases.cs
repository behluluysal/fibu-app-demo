using App.Repository;
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
using System.Threading.Tasks;

namespace UseCases
{
    public class ApplicationUsersScreenUseCases : IApplicationUsersScreenUseCases
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IBusinessPartnerRepository _businessPartnerRepository;
        private readonly IBPRPEmailRepository _bPRPEmailRepository;

        public ApplicationUsersScreenUseCases(IApplicationUserRepository userRepository,
            IBusinessPartnerRepository businessPartnerRepository,
            IBPRPEmailRepository bPRPEmailRepository)
        {
            _userRepository = userRepository;
            _businessPartnerRepository = businessPartnerRepository;
            _bPRPEmailRepository = bPRPEmailRepository;
        }


        #region User Crud Operations

        public async Task<(int, IEnumerable<ApplicationUserDto>)> ViewUsersAsync(QueryParams qp)
        {
            return await _userRepository.GetAsync(qp);
        }
        public async Task<ApplicationUserDto> CreateUserAsync(CreateUserViewModel user)
        {
            return await _userRepository.CreateAsync(user);
        }

        public async Task<ApplicationUserDto> ViewUserByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(CreateUserViewModel user, string id)
        {
            //Get the old email
            ApplicationUserWithRoleDto userDto = await _userRepository.GetUserRoles(id);


            //Find if there is a business partner with this email
            var response = await _businessPartnerRepository.GetAsync(new QueryParams());
            IEnumerable<BusinessPartnerDto> businessPartners = response.Item2;
            BusinessPartnerDto businessPartner = businessPartners.FirstOrDefault(x=>x.Email  == userDto.Email);

            if (userDto.Roles.Any(x=>x.Name == "BusinessPartner"))
            {
                //If found businessPartner, update email
                if (businessPartner != null)
                {
                    businessPartner.Email = user.Email;
                    await _businessPartnerRepository.UpdateAsync(businessPartner.Id, businessPartner);
                }
                else
                {
                    //Find if there is a BPRPEmail with this email
                    var response2 = await _bPRPEmailRepository.GetAsync(new QueryParams());
                    IEnumerable<BPRPEmailDto> bPRPEmailS = response2.Item2;
                    BPRPEmailDto bPRPEmail = bPRPEmailS.FirstOrDefault(x => x.Email == userDto.Email);

                    if (bPRPEmail != null && bPRPEmail.CanLogin)
                    {
                        bPRPEmail.Email = user.Email;
                        await _bPRPEmailRepository.UpdateAsync(bPRPEmail, bPRPEmail.Id);
                    }
                }
            }
            await _userRepository.UpdateAsync(user, id);
        }

        public async Task DeleteUserAsync(string id)
        {
            //Get the old email
            ApplicationUserWithRoleDto userDto = await _userRepository.GetUserRoles(id);


            //Find if there is a business partner with this email
            var response = await _businessPartnerRepository.GetAsync(new QueryParams());
            IEnumerable<BusinessPartnerDto> businessPartners = response.Item2;
            BusinessPartnerDto businessPartner = businessPartners.FirstOrDefault(x => x.Email == userDto.Email);

            if (userDto.Roles.Any(x => x.Name == "BusinessPartner"))
            {
                //If found businessPartner, delete email
                if (businessPartner != null)
                {
                    throw new Exception($"Record with email {userDto.Username} is a Business Partner!");
                }
                else
                {
                    //Find if there is a BPRPEmail with this email
                    var response2 = await _bPRPEmailRepository.GetAsync(new QueryParams());
                    IEnumerable<BPRPEmailDto> bPRPEmailS = response2.Item2;
                    BPRPEmailDto bPRPEmail = bPRPEmailS.FirstOrDefault(x => x.Email == userDto.Email);

                    if (bPRPEmail != null && bPRPEmail.CanLogin)
                    {
                        await _bPRPEmailRepository.DeleteAsync(bPRPEmail.Id);
                    }
                }
            }
            await _userRepository.DeleteAsync(id);
        }

        #endregion



        #region User Role Operations

        public async Task<ApplicationUserWithRoleDto> GetRolesAsync(string id)
        {
            return await _userRepository.GetUserRoles(id);
        }

        public async Task<ApplicationUserWithRoleDto> AssignRoleToUser(string userId, string roleId)
        {
            return await _userRepository.AssignRoleAsync(userId, roleId);
        }

        public async Task WithdrawRoleFromUser(string userId, string roleId)
        {
            await _userRepository.WithdrawRoleFromUserAsync(userId, roleId);
        }

        #endregion



    }
}
