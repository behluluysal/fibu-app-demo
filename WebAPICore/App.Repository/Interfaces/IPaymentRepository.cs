using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repository
{
    public interface IPaymentRepository
    {
        Task<PaymentDto> CreateAsync(PaymentDto product);
        Task DeleteAsync(int id);
        Task<(int, IEnumerable<PaymentDto>)> GetAsync(QueryParams qp);
        Task<PaymentDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, PaymentDto product);
    }
}