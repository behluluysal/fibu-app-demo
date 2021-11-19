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
    public class RequestsScreenUseCases : IRequestsScreenUseCases
    {
        private readonly IRequestRepository _requestRepository;

        public RequestsScreenUseCases(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        #region CRUD Methods

        public async Task<(int, IEnumerable<RequestDto>)> ViewRequestsAsync(QueryParams qp)
        {
            return await _requestRepository.GetAsync(qp);
        }

        public async Task<RequestDto> CreateRequestAsync(RequestDto request)
        {
            return await _requestRepository.CreateAsync(request);
        }

        public async Task<RequestDto> ViewRequestByIdAsync(int id)
        {
            return await _requestRepository.GetByIdAsync(id);
        }
        public async Task<RequestDto> ViewRequestByTokenAsync(string token)
        {
            var response = await _requestRepository.GetAsync(new QueryParams() { Filter=$"(Token == \"{token}\")"});
            IEnumerable<RequestDto> requests = response.Item2;
            return requests.ElementAtOrDefault(0);
        }

        public async Task UpdateRequestAsync(int id, RequestDto request)
        {
            await _requestRepository.UpdateAsync(request, id);
        }

        public async Task DeleteRequestAsync(int id)
        {
            await _requestRepository.DeleteAsync(id);
        }

        #endregion
    }
}
