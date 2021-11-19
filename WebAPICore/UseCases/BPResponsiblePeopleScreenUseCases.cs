using App.Repository;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases
{
    public class BPResponsiblePeopleScreenUseCases : IBPResponsiblePeopleScreenUseCases
    {
        private readonly IBPResponsiblePersonRepository _bprpesponsiblePersonRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IBPRPEmailRepository _bPRPEmailRepository;

        public BPResponsiblePeopleScreenUseCases(IBPResponsiblePersonRepository BPResponsiblePersonRepository,
            IApplicationUserRepository applicationUserRepository,
            IBPRPEmailRepository bPRPEmailRepository)
        {
            _bprpesponsiblePersonRepository = BPResponsiblePersonRepository;
            _applicationUserRepository = applicationUserRepository;
            _bPRPEmailRepository = bPRPEmailRepository;
        }



        #region CRUD UseCases

        public async Task<(int, IEnumerable<BPResponsiblePersonDto>)> ViewResponsiblePeopleAsync(QueryParams qp)
        {
            return await _bprpesponsiblePersonRepository.GetAsync(qp);
        }

        public async Task<BPResponsiblePersonDto> CreateResponsiblePeopleAsync(BPResponsiblePersonDto BPResponsiblePerson)
        {
            return await _bprpesponsiblePersonRepository.CreateAsync(BPResponsiblePerson);
        }

        public async Task<BPResponsiblePersonDto> ViewResponsiblePeopleByIdAsync(int id)
        {
            return await _bprpesponsiblePersonRepository.GetByIdAsync(id);
        }

        public async Task UpdateResponsiblePeopleAsync(int id, BPResponsiblePersonDto BPResponsiblePerson)
        {
            await _bprpesponsiblePersonRepository.UpdateAsync(id, BPResponsiblePerson);
        }

        public async Task DeleteResponsiblePeopleAsync(int id)
        {
            await _bprpesponsiblePersonRepository.DeleteAsync(id);
        }

        #endregion


        #region BPResponsiblePerson PhoneNumber UseCases

        public async Task<BPResponsiblePersonWithPhoneNumberDto> ViewBPResponsiblePersonWithGsmsAsync(int id, QueryParams qp)
        {
            return await _bprpesponsiblePersonRepository.GetByIdWithGsmAsync(id, qp);
        }
        public async Task<BPRPPhoneNumberDto> CreatePhoneNumberToBPRP(BPRPPhoneNumberDto phoneNumberDto)
        {
            return await _bprpesponsiblePersonRepository.CreateGsmAsync(phoneNumberDto);
        }
        public async Task UpdatePhoneNumberToBPRP(int phoneNumberId, BPRPPhoneNumberDto numberDto)
        {
            await _bprpesponsiblePersonRepository.UpdateGsmAsync(phoneNumberId, numberDto);
        }
        public async Task DeletePhoneNumberToBPRP(int CompanyId, int phoneNumberId)
        {
            await _bprpesponsiblePersonRepository.DeleteGsmAsync(CompanyId, phoneNumberId);
        }

        #endregion

        #region ReponsiblePerson Email UseCases

        public async Task<BPResponsiblePersonWithEmailDto> ViewBPResponsiblePersonWithEmailsAsync(int id, QueryParams qp)
        {
            return await _bprpesponsiblePersonRepository.GetByIdWithEmailAsync(id, qp);
        }

        public async Task<BPRPEmailDto> CreateEmailToBPRP(BPRPEmailDto emailDto)
        {

            if (emailDto.CanLogin == true)
            {
                BPResponsiblePersonWithEmailDto bPResponsible = await _bprpesponsiblePersonRepository.GetByIdWithEmailAsync(emailDto.ResponsiblePersonId, new QueryParams());
                BPRPEmailDto CanLoginEmail = bPResponsible.Emails.Where(x => x.CanLogin == true).FirstOrDefault();

                if (CanLoginEmail != null)
                {
                    throw new Exception($"Record with id {bPResponsible.Id} has aldready login credential");
                }
                else
                {
                    BPRPEmailDto createdEmail = await _bprpesponsiblePersonRepository.CreateEmailAsync(emailDto);
                    CreateUserViewModel newUser = new CreateUserViewModel { Email = emailDto.Email, UserName = emailDto.Email, Password = "1234567", ConfirmPassword = "1234567" };
                    await _applicationUserRepository.CreateAsync(newUser);
                    return createdEmail;
                }
            }
            return await _bprpesponsiblePersonRepository.CreateEmailAsync(emailDto);
        }
        public async Task UpdateEmailToBPRP(int emailId, BPRPEmailDto emailDto)
        {
            BPRPEmailDto oldEmail = await _bPRPEmailRepository.GetByIdAsync(emailId);

            var response = await _applicationUserRepository.GetAsync(new QueryParams());
            IEnumerable<ApplicationUserDto> users = response.Item2;
            ApplicationUserDto user = users.Where(x => x.Email == oldEmail.Email).FirstOrDefault();

            await _bprpesponsiblePersonRepository.UpdateEmailAsync(emailId, emailDto);
            if (user != null)
            {
                CreateUserViewModel newUser = new CreateUserViewModel { Email = emailDto.Email, UserName = emailDto.Email, Password = "dummypassword", ConfirmPassword = "dummypassword" };
                await _applicationUserRepository.UpdateAsync(newUser, user.Id);
            }
        }

        public async Task DeleteEmailToBPRP(int CompanyId, int emailId)
        {
            BPRPEmailDto email = await _bPRPEmailRepository.GetByIdAsync(emailId);
            var response = await _applicationUserRepository.GetAsync(new QueryParams());
            IEnumerable<ApplicationUserDto> users = response.Item2;

            ApplicationUserDto user = users.Where(x => x.Email == email.Email).FirstOrDefault();
            if (user != null)
            {
                await _applicationUserRepository.DeleteAsync(user.Id);

            }

            await _bprpesponsiblePersonRepository.DeleteEmailAsync(CompanyId, emailId);
        }

        #endregion
    }
}
