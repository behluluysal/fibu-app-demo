using App.Repository;
using Core.AutoMapperDtos;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases
{
    public class RequestedProductsScreenUseCases : IRequestedProductsScreenUseCases
    {
        private readonly IRequestedProductRepository _requestedProductRepository;
        public RequestedProductsScreenUseCases(IRequestedProductRepository requestedProductRepository)
        {
            _requestedProductRepository = requestedProductRepository;
        }



        #region CRUD Methods

        public async Task<(int, IEnumerable<RequestedProductDto>)> ViewRequestedProductsAsync(QueryParams qp)
        {
            return await _requestedProductRepository.GetAsync(qp);
        }

        public async Task<RequestedProductDto> CreateRequestedProductAsync(RequestedProductDto company)
        {
            return await _requestedProductRepository.CreateAsync(company);
        }

        public async Task<RequestedProductDto> ViewRequestedProductByIdAsync(int id)
        {
            return await _requestedProductRepository.GetByIdAsync(id);
        }

        public async Task UpdateRequestedProductAsync(int id, RequestedProductDto company)
        {
            await _requestedProductRepository.UpdateAsync(id, company);
        }

        public async Task DeleteRequestedProductAsync(int id)
        {
            await _requestedProductRepository.DeleteAsync(id);
        }

        #endregion
    }
}
