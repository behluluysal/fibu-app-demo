using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IBPRPEmailRepository
    {
        Task<BPRPEmailDto> CreateAsync(BPRPEmailDto bprpemail);
        Task DeleteAsync(int id);
        Task<(int, IEnumerable<BPRPEmailDto>)> GetAsync(QueryParams qp);
        Task<BPRPEmailDto> GetByIdAsync(int id);
        Task UpdateAsync(BPRPEmailDto bprpemail, int id);
    }
}