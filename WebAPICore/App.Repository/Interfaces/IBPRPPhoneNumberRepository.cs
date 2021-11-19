using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IBPRPPhoneNumberRepository
    {
        Task<(int, IEnumerable<BPRPPhoneNumberDto>)> GetAsync(QueryParams qp);
        Task<BPRPPhoneNumberDto> CreateAsync(BPRPPhoneNumberDto scrpPhoneNumber);
        Task<BPRPPhoneNumberDto> GetByIdAsync(int id);
        Task UpdateAsync(BPRPPhoneNumberDto scrpPhoneNumber, int id);
        Task DeleteAsync(int id);
    }
}