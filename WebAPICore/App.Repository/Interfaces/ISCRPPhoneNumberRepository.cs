using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface ISCRPPhoneNumberRepository
    {
        Task<(int, IEnumerable<SCRPPhoneNumberDto>)> GetAsync(QueryParams qp);
        Task<SCRPPhoneNumberDto> CreateAsync(SCRPPhoneNumberDto scrpPhoneNumber);
        Task<SCRPPhoneNumberDto> GetByIdAsync(int id);
        Task UpdateAsync(SCRPPhoneNumberDto scrpPhoneNumber, int id);
        Task DeleteAsync(int id);
    }
}