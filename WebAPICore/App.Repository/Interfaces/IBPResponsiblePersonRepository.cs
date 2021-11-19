using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IBPResponsiblePersonRepository
    {
        Task<BPResponsiblePersonDto> CreateAsync(BPResponsiblePersonDto responsiblePerson);
        Task<BPRPEmailDto> CreateEmailAsync(BPRPEmailDto email);
        Task<BPRPPhoneNumberDto> CreateGsmAsync(BPRPPhoneNumberDto gsm);
        Task DeleteAsync(int id);
        Task DeleteEmailAsync(int BPResponsiblePersonId, int EmailId);
        Task DeleteGsmAsync(int BPResponsiblePersonId, int GsmId);
        Task<(int, IEnumerable<BPResponsiblePersonDto>)> GetAsync(QueryParams qp);
        Task<BPResponsiblePersonDto> GetByIdAsync(int id);
        Task<BPResponsiblePersonWithEmailDto> GetByIdWithEmailAsync(int id, QueryParams qp);
        Task<BPResponsiblePersonWithPhoneNumberDto> GetByIdWithGsmAsync(int id, QueryParams qp);
        Task UpdateAsync(int id, BPResponsiblePersonDto responsiblePerson);
        Task UpdateEmailAsync(int emailId, BPRPEmailDto email);
        Task UpdateGsmAsync(int phoneNumberId, BPRPPhoneNumberDto gsm);
    }
}