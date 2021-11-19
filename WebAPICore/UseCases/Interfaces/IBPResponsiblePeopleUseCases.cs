using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IBPResponsiblePeopleScreenUseCases
    {
        Task<BPRPEmailDto> CreateEmailToBPRP(BPRPEmailDto emailDto);
        Task<BPRPPhoneNumberDto> CreatePhoneNumberToBPRP(BPRPPhoneNumberDto phoneNumberDto);
        Task<BPResponsiblePersonDto> CreateResponsiblePeopleAsync(BPResponsiblePersonDto BPResponsiblePerson);
        Task DeleteEmailToBPRP(int CompanyId, int emailId);
        Task DeletePhoneNumberToBPRP(int CompanyId, int phoneNumberId);
        Task DeleteResponsiblePeopleAsync(int id);
        Task UpdateEmailToBPRP(int phoneNumberId, BPRPEmailDto emailDto);
        Task UpdatePhoneNumberToBPRP(int phoneNumberId, BPRPPhoneNumberDto numberDto);
        Task UpdateResponsiblePeopleAsync(int id, BPResponsiblePersonDto BPResponsiblePerson);
        Task<BPResponsiblePersonWithEmailDto> ViewBPResponsiblePersonWithEmailsAsync(int id, QueryParams qp);
        Task<BPResponsiblePersonWithPhoneNumberDto> ViewBPResponsiblePersonWithGsmsAsync(int id, QueryParams qp);
        Task<(int, IEnumerable<BPResponsiblePersonDto>)> ViewResponsiblePeopleAsync(QueryParams qp);
        Task<BPResponsiblePersonDto> ViewResponsiblePeopleByIdAsync(int id);
    }
}