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
    internal class SCRPEmailsScreenUseCases : ISCRPEmailsScreenUseCases
    {
        private readonly ISCRPEmailRepository _scrpEmailRepository;

        public SCRPEmailsScreenUseCases(ISCRPEmailRepository scrpEmailRepository)
        {
            _scrpEmailRepository = scrpEmailRepository;
        }

        #region CRUD Methods

        public async Task<(int, IEnumerable<SCRPEmailDto>)> ViewSCRPEmailsAsync(QueryParams qp)
        {
             return await _scrpEmailRepository.GetAsync(qp);
        }

        public async Task<SCRPEmailDto> CreateSCRPEmailAsync(SCRPEmailDto scrpEmail)
        {
            return await _scrpEmailRepository.CreateAsync(scrpEmail);
        }

        public async Task<SCRPEmailDto> ViewSCRPEmailByIdAsync(int id)
        {
            return await _scrpEmailRepository.GetByIdAsync(id);
        }

        public async Task UpdateSCRPEmailAsync(int id, SCRPEmailDto scrpEmail)
        {
            await _scrpEmailRepository.UpdateAsync(scrpEmail, id);
        }

        public async Task DeleteSCRPEmailAsync(int id)
        {
            await _scrpEmailRepository.DeleteAsync(id);
        }

        #endregion
    }
}
