using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IResponsiblePersonRepository
    {
        Task<(int, IEnumerable<ResponsiblePersonDto>)> GetAsync(QueryParams qp);
        Task<ResponsiblePersonDto> CreateAsync(ResponsiblePersonDto responsiblePerson);
        Task<ResponsiblePersonDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, ResponsiblePersonDto responsiblePerson);
        Task DeleteAsync(int id);


        Task<(int, IEnumerable<ResponsiblePersonWithContactDto>)> GetWithContactAsync(QueryParams qp);
        Task<ResponsiblePersonWithContactDto> GetByIdWithContactAsync(int id);

        Task<(int, ResponsiblePersonWithEmailDto)> GetByIdWithEmailAsync(int id, QueryParams qp);
        Task<SCRPEmailDto> CreateEmailAsync(SCRPEmailDto email);
        Task UpdateEmailAsync(int emailId, SCRPEmailDto email);
        Task DeleteEmailAsync(int ResponsiblePersonId, int EmailId);


        Task<(int, ResponsiblePersonWithPhoneNumberDto)> GetByIdWithGsmAsync(int id, QueryParams qp);
        Task<SCRPPhoneNumberDto> CreateGsmAsync(SCRPPhoneNumberDto gsm);
        Task UpdateGsmAsync(int phoneNumberId, SCRPPhoneNumberDto gsm);
        Task DeleteGsmAsync(int ResponsiblePersonId, int GsmId);
        
    }
}