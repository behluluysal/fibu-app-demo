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
    internal class BPRPPhoneNumbersScreenUseCases : IBPRPPhoneNumbersScreenUseCases
    {
        private readonly IBPRPPhoneNumberRepository _scrpPhoneNumberRepository;

        public BPRPPhoneNumbersScreenUseCases(IBPRPPhoneNumberRepository scrpPhoneNumberRepository)
        {
            _scrpPhoneNumberRepository = scrpPhoneNumberRepository;
        }

        #region CRUD Methods

        public async Task<(int, IEnumerable<BPRPPhoneNumberDto>)> ViewBPRPPhoneNumbersAsync(QueryParams qp)
        {
            return await _scrpPhoneNumberRepository.GetAsync(qp);
        }

        public async Task<BPRPPhoneNumberDto> CreateBPRPPhoneNumberAsync(BPRPPhoneNumberDto scrpPhoneNumber)
        {
            return await _scrpPhoneNumberRepository.CreateAsync(scrpPhoneNumber);
        }

        public async Task<BPRPPhoneNumberDto> ViewBPRPPhoneNumberByIdAsync(int id)
        {
            return await _scrpPhoneNumberRepository.GetByIdAsync(id);
        }

        public async Task UpdateBPRPPhoneNumberAsync(BPRPPhoneNumberDto scrpPhoneNumber, int id)
        {
            await _scrpPhoneNumberRepository.UpdateAsync(scrpPhoneNumber, id);
        }

        public async Task DeleteBPRPPhoneNumberAsync(int id)
        {
            await _scrpPhoneNumberRepository.DeleteAsync(id);
        }

        #endregion
    }
}
