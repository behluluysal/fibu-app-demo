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
    internal class SCRPPhoneNumbersScreenUseCases : ISCRPPhoneNumbersScreenUseCases
    {
        private readonly ISCRPPhoneNumberRepository _scrpPhoneNumberRepository;

        public SCRPPhoneNumbersScreenUseCases(ISCRPPhoneNumberRepository scrpPhoneNumberRepository)
        {
            _scrpPhoneNumberRepository = scrpPhoneNumberRepository;
        }

        #region CRUD Methods

        public async Task<(int, IEnumerable<SCRPPhoneNumberDto>)> ViewSCRPPhoneNumbersAsync(QueryParams qp)
        {
            return await _scrpPhoneNumberRepository.GetAsync(qp);
        }

        public async Task<SCRPPhoneNumberDto> CreateSCRPPhoneNumberAsync(SCRPPhoneNumberDto scrpPhoneNumber)
        {
            return await _scrpPhoneNumberRepository.CreateAsync(scrpPhoneNumber);
        }

        public async Task<SCRPPhoneNumberDto> ViewSCRPPhoneNumberByIdAsync(int id)
        {
            return await _scrpPhoneNumberRepository.GetByIdAsync(id);
        }

        public async Task UpdateSCRPPhoneNumberAsync(SCRPPhoneNumberDto scrpPhoneNumber, int id)
        {
            await _scrpPhoneNumberRepository.UpdateAsync(scrpPhoneNumber, id);
        }

        public async Task DeleteSCRPPhoneNumberAsync(int id)
        {
            await _scrpPhoneNumberRepository.DeleteAsync(id);
        }

        #endregion
    }
}
