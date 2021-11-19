using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface ISCRPPhoneNumbersScreenUseCases
    {
        Task<SCRPPhoneNumberDto> CreateSCRPPhoneNumberAsync(SCRPPhoneNumberDto scrpPhoneNumber);
        Task DeleteSCRPPhoneNumberAsync(int id);
        Task UpdateSCRPPhoneNumberAsync(SCRPPhoneNumberDto scrpPhoneNumber, int id);
        Task<SCRPPhoneNumberDto> ViewSCRPPhoneNumberByIdAsync(int id);
        Task<(int, IEnumerable<SCRPPhoneNumberDto>)> ViewSCRPPhoneNumbersAsync(QueryParams qp);
    }
}