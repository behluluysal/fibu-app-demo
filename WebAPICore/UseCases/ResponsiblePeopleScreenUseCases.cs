using App.Repository;
using Core.AutoMapperDtos;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases
{
    public class ResponsiblePeopleScreenUseCases : IResponsiblePeopleScreenUseCases
    {
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;
        public ResponsiblePeopleScreenUseCases(IResponsiblePersonRepository responsiblePersonRepository)
        {
            _responsiblePersonRepository = responsiblePersonRepository;
        }



        #region CRUD UseCases

        public async Task<(int, IEnumerable<ResponsiblePersonDto>)> ViewResponsiblePeopleAsync(QueryParams qp)
        {
            return await _responsiblePersonRepository.GetAsync(qp);
        }

        public async Task<ResponsiblePersonDto> CreateResponsiblePeopleAsync(ResponsiblePersonDto responsibleperson)
        {
            return await _responsiblePersonRepository.CreateAsync(responsibleperson);
        }

        public async Task<ResponsiblePersonDto> ViewResponsiblePeopleByIdAsync(int id)
        {
            return await _responsiblePersonRepository.GetByIdAsync(id);
        }

        public async Task UpdateResponsiblePeopleAsync(int id, ResponsiblePersonDto responsibleperson)
        {
            await _responsiblePersonRepository.UpdateAsync(id, responsibleperson);
        }

        public async Task DeleteResponsiblePeopleAsync(int id)
        {
            await _responsiblePersonRepository.DeleteAsync(id);
        }

        #endregion

        #region ResponsiblePerson Contact UseCases

        public async Task<(int, IEnumerable<ResponsiblePersonWithContactDto>)> ViewResponsiblePeopleWithContactAsync(QueryParams qp)
        {
            return await _responsiblePersonRepository.GetWithContactAsync(qp);
        }
        public async Task<ResponsiblePersonWithContactDto> ViewResponsiblePersonByIdWithContactAsync(int id)
        {
            return await _responsiblePersonRepository.GetByIdWithContactAsync(id);
        }

        #endregion

        #region ResponsiblePerson PhoneNumber UseCases

        public async Task<(int, ResponsiblePersonWithPhoneNumberDto)> ViewResponsiblePersonWithGsmsAsync(int id, QueryParams qp)
        {
            return await _responsiblePersonRepository.GetByIdWithGsmAsync(id, qp);
        }
        public async Task<SCRPPhoneNumberDto> CreatePhoneNumberToSCRP(SCRPPhoneNumberDto phoneNumberDto)
        {
            return await _responsiblePersonRepository.CreateGsmAsync(phoneNumberDto);
        }
        public async Task UpdatePhoneNumberToSCRP( int phoneNumberId, SCRPPhoneNumberDto numberDto)
        {
            await _responsiblePersonRepository.UpdateGsmAsync(phoneNumberId, numberDto);
        }
        public async Task DeletePhoneNumberToSCRP(int CompanyId, int phoneNumberId)
        {
            await _responsiblePersonRepository.DeleteGsmAsync(CompanyId, phoneNumberId);
        }

        #endregion

        #region ReponsiblePerson Email UseCases

        public async Task<(int, ResponsiblePersonWithEmailDto)> ViewResponsiblePersonWithEmailsAsync(int id, QueryParams qp)
        {
            return await _responsiblePersonRepository.GetByIdWithEmailAsync(id, qp);
        }

        public async Task<SCRPEmailDto> CreateEmailToSCRP(SCRPEmailDto emailDto)
        {
            return await _responsiblePersonRepository.CreateEmailAsync(emailDto);
        }
        public async Task UpdateEmailToSCRP(int phoneNumberId, SCRPEmailDto emailDto)
        {
            await _responsiblePersonRepository.UpdateEmailAsync(phoneNumberId, emailDto);
        }

        public async Task DeleteEmailToSCRP(int CompanyId, int emailId)
        {
            await _responsiblePersonRepository.DeleteEmailAsync(CompanyId, emailId);
        }

        #endregion

    }
}
