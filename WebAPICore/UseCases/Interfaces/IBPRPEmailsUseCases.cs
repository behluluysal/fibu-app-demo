using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IBPRPEmailsScreenUseCases
    {
        Task<BPRPEmailDto> CreateBPRPEmailAsync(BPRPEmailDto scrpEmail);
        Task DeleteBPRPEmailAsync(int id);
        Task UpdateBPRPEmailAsync(BPRPEmailDto scrpEmail, int id);
        Task<BPRPEmailDto> ViewBPRPEmailByIdAsync(int id);
        Task<(int, IEnumerable<BPRPEmailDto>)> ViewBPRPEmailsAsync(QueryParams qp);
    }
}