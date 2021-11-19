using App.Repository;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UseCases
{
    public class TagsScreenUseCases : ITagsScreenUseCases
    {
        private readonly ITagRepository _tagRepository;

        public TagsScreenUseCases(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        #region CRUD Methods

        public async Task<(int, IEnumerable<TagDto>)> ViewTagsAsync(QueryParams qp)
        {
            return await _tagRepository.GetAsync(qp);
        }

        public async Task<TagDto> CreateTagAsync(TagDto tag)
        {
            return await _tagRepository.CreateAsync(tag);
        }

        public async Task<TagDto> ViewTagByIdAsync(int id)
        {
            return await _tagRepository.GetByIdAsync(id);
        }

        public async Task UpdateTagAsync(int id, TagDto tag)
        {
            await _tagRepository.UpdateAsync(id,tag);
        }

        public async Task DeleteTagAsync(int id)
        {
            await _tagRepository.DeleteAsync(id);
        }

        #endregion
    }
}
