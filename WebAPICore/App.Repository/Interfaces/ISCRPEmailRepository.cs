using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface ISCRPEmailRepository
    {
        Task<(int, IEnumerable<SCRPEmailDto>)> GetAsync(QueryParams qp);
        Task<SCRPEmailDto> CreateAsync(SCRPEmailDto scrpemail);
        Task<SCRPEmailDto> GetByIdAsync(int id);
        Task UpdateAsync(SCRPEmailDto scrpemail, int id);
        Task DeleteAsync(int id);
    }
}