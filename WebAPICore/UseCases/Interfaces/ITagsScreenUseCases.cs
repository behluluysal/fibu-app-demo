using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface ITagsScreenUseCases
    {
        Task<(int, IEnumerable<TagDto>)> ViewTagsAsync(QueryParams qp);
        Task<TagDto> CreateTagAsync(TagDto tag);
        Task<TagDto> ViewTagByIdAsync(int id);
        Task UpdateTagAsync(int id, TagDto tag);
        Task DeleteTagAsync(int id);
    }
}