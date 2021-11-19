using App.Repository;
using Core.AutoMapperDtos;
using Core.Pagination;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UseCases
{
    public class BusinessPartnersScreenUseCases : IBusinessPartnersScreenUseCases
    {
        private readonly IBusinessPartnerRepository _businessPartnerRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public BusinessPartnersScreenUseCases(IBusinessPartnerRepository businessPartnerRepository,
            IApplicationUserRepository applicationUserRepository)
        {
            _businessPartnerRepository = businessPartnerRepository;
            _applicationUserRepository = applicationUserRepository;
        }



        #region CRUD Methods

        public async Task<(int, IEnumerable<BusinessPartnerDto>)> ViewBusinessPartnersAsync(QueryParams qp)
        {
            return await _businessPartnerRepository.GetAsync(qp);
        }

        public async Task<BusinessPartnerDto> CreateBusinessPartnerAsync(BusinessPartnerDto company)
        {
            CreateUserViewModel newUser = new CreateUserViewModel { Email = company.Email, UserName = company.Email, Password = "1234567", ConfirmPassword = "1234567" };
            await _applicationUserRepository.CreateAsync(newUser);

            return await _businessPartnerRepository.CreateAsync(company);
        }

        public async Task<BusinessPartnerDto> ViewBusinessPartnerByIdAsync(int id)
        {
            return await _businessPartnerRepository.GetByIdAsync(id);
        }

        public async Task UpdateBusinessPartnerAsync(int id, BusinessPartnerDto company)
        {
            BusinessPartnerDto businessPartner = await _businessPartnerRepository.GetByIdAsync(id);

            var response2 = await _applicationUserRepository.GetAsync(new QueryParams());
            IEnumerable<ApplicationUserDto> users = response2.Item2;
            ApplicationUserDto user = users.Where(x => x.Email == businessPartner.Email).FirstOrDefault();
            if (user != null)
            {
                CreateUserViewModel newUser = new CreateUserViewModel { Email = company.Email, UserName = company.Email, Password = "dummypassword", ConfirmPassword= "dummypassword" };
                await _applicationUserRepository.UpdateAsync(newUser, user.Id);
            }

            await _businessPartnerRepository.UpdateAsync(id, company);
        }

        public async Task DeleteBusinessPartnerAsync(int id)
        {
            BusinessPartnerDto businessPartner = await _businessPartnerRepository.GetByIdAsync(id);

            var response2 = await _applicationUserRepository.GetAsync(new QueryParams());
            IEnumerable<ApplicationUserDto> users = response2.Item2;
            ApplicationUserDto user = users.Where(x => x.Email == businessPartner.Email).FirstOrDefault();
            if (user != null)
            {
                await _applicationUserRepository.DeleteAsync(user.Id);
            }

            await _businessPartnerRepository.DeleteAsync(id);
        }

        #endregion


        #region BusinessPartner ResponsiblePerson Methods

        public async Task<(int, BusinessPartnerWithResponsiblePersonDto)> ViewBusinessPartnerResponsiblePeoplesAsync(int id, QueryParams qp)
        {
            return await _businessPartnerRepository.GetWithResponsiblePeopleAsync(id, qp);
        }

        #endregion

        #region BusinessPartner RequestedProduct Methods

        public async Task<(int, BusinessPartnerWithRequestsDto)> ViewBusinessPartnerRequestsAsync(int id, QueryParams qp)
        {
            return await _businessPartnerRepository.GetWithRequestsAsync(id, qp);
        }


        public async Task<BusinessPartnerWithRequestWithRequestedProductsDto> ViewBusinessPartnerRequestWithRequestedProductsAsync(int id, int rid)
        {
            return await _businessPartnerRepository.GetWithRequestWithRequestedProductsAsync(id, rid);
        }
        #endregion
    }
}
