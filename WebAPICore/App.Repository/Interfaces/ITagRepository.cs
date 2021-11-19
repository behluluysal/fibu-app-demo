using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface ITagRepository
    {
        Task<(int, IEnumerable<TagDto>)> GetAsync(QueryParams qp);
        Task<TagDto> CreateAsync(TagDto tag);
        Task<TagDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, TagDto tag);
        Task DeleteAsync(int id);
    }
}