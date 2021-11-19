using Core.AutoMapperDtos;
using Core.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UseCases
{
    public interface IPaymentsScreenUseCases
    {
        Task<PaymentDto> CreatePaymentAsync(PaymentDto company);
        Task DeletePaymentAsync(int id);
        Task UpdatePaymentAsync(int id, PaymentDto company);
        Task<PaymentDto> ViewPaymentByIdAsync(int id);
        Task<(int, IEnumerable<PaymentDto>)> ViewPaymentsAsync(QueryParams qp);
    }
}