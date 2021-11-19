using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IResponsiblePeopleScreenUseCases
    {
        Task<(int, IEnumerable<ResponsiblePersonDto>)> ViewResponsiblePeopleAsync(QueryParams qp);
        Task<ResponsiblePersonDto> CreateResponsiblePeopleAsync(ResponsiblePersonDto responsibleperson);
        Task<ResponsiblePersonDto> ViewResponsiblePeopleByIdAsync(int id);
        Task UpdateResponsiblePeopleAsync(int id, ResponsiblePersonDto responsibleperson);
        Task DeleteResponsiblePeopleAsync(int id);


        Task<(int, IEnumerable<ResponsiblePersonWithContactDto>)> ViewResponsiblePeopleWithContactAsync(QueryParams qp);
        Task<ResponsiblePersonWithContactDto> ViewResponsiblePersonByIdWithContactAsync(int id);


        Task<SCRPEmailDto> CreateEmailToSCRP(SCRPEmailDto emailDto);
        Task UpdateEmailToSCRP(int emailId, SCRPEmailDto emailDto);
        Task DeleteEmailToSCRP(int ResponsiblePersonId, int emailId);


        Task<SCRPPhoneNumberDto> CreatePhoneNumberToSCRP(SCRPPhoneNumberDto phoneNumberDto);
        Task UpdatePhoneNumberToSCRP(int phoneNumberId, SCRPPhoneNumberDto numberDto);
        Task DeletePhoneNumberToSCRP(int ResponsiblePersonId, int phoneNumberId);
        Task<(int, ResponsiblePersonWithPhoneNumberDto)> ViewResponsiblePersonWithGsmsAsync(int id, QueryParams qp);
        Task<(int, ResponsiblePersonWithEmailDto)> ViewResponsiblePersonWithEmailsAsync(int id, QueryParams qp);
    }
}