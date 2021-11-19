using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface ISCRPEmailsScreenUseCases
    {
        Task<SCRPEmailDto> CreateSCRPEmailAsync(SCRPEmailDto scrpEmail);
        Task DeleteSCRPEmailAsync(int id);
        Task UpdateSCRPEmailAsync(int id, SCRPEmailDto scrpEmail);
        Task<SCRPEmailDto> ViewSCRPEmailByIdAsync(int id);
        Task<(int, IEnumerable<SCRPEmailDto>)> ViewSCRPEmailsAsync(QueryParams qp);
    }
}