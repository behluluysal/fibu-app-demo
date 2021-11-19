using App.Repository;
using Core.AutoMapperDtos;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UseCases
{
    internal class BPRPEmailsScreenUseCases : IBPRPEmailsScreenUseCases
    {
        private readonly IBPRPEmailRepository _scrpEmailRepository;

        public BPRPEmailsScreenUseCases(IBPRPEmailRepository scrpEmailRepository)
        {
            _scrpEmailRepository = scrpEmailRepository;
        }

        #region CRUD Methods

        public async Task<(int, IEnumerable<BPRPEmailDto>)> ViewBPRPEmailsAsync(QueryParams qp)
        {
            return await _scrpEmailRepository.GetAsync(qp);
        }

        public async Task<BPRPEmailDto> CreateBPRPEmailAsync(BPRPEmailDto scrpEmail)
        {
            return await _scrpEmailRepository.CreateAsync(scrpEmail);
        }

        public async Task<BPRPEmailDto> ViewBPRPEmailByIdAsync(int id)
        {
            return await _scrpEmailRepository.GetByIdAsync(id);
        }

        public async Task UpdateBPRPEmailAsync(BPRPEmailDto scrpEmail, int id)
        {
            await _scrpEmailRepository.UpdateAsync(scrpEmail, id);
        }

        public async Task DeleteBPRPEmailAsync(int id)
        {
            await _scrpEmailRepository.DeleteAsync(id);
        }

        #endregion
    }
}
