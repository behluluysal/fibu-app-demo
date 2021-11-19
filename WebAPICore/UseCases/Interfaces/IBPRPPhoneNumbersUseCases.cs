using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    internal interface IBPRPPhoneNumbersScreenUseCases
    {
        Task<BPRPPhoneNumberDto> CreateBPRPPhoneNumberAsync(BPRPPhoneNumberDto scrpPhoneNumber);
        Task DeleteBPRPPhoneNumberAsync(int id);
        Task UpdateBPRPPhoneNumberAsync(BPRPPhoneNumberDto scrpPhoneNumber, int id);
        Task<BPRPPhoneNumberDto> ViewBPRPPhoneNumberByIdAsync(int id);
        Task<(int, IEnumerable<BPRPPhoneNumberDto>)> ViewBPRPPhoneNumbersAsync(QueryParams qp);
    }
}