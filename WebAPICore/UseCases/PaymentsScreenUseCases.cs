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
    public class PaymentsScreenUseCases : IPaymentsScreenUseCases
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentsScreenUseCases(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }



        #region CRUD Methods

        public async Task<(int, IEnumerable<PaymentDto>)> ViewPaymentsAsync(QueryParams qp)
        {
            return await _paymentRepository.GetAsync(qp);
        }

        public async Task<PaymentDto> CreatePaymentAsync(PaymentDto company)
        {
            return await _paymentRepository.CreateAsync(company);
        }

        public async Task<PaymentDto> ViewPaymentByIdAsync(int id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task UpdatePaymentAsync(int id, PaymentDto company)
        {
            await _paymentRepository.UpdateAsync(id, company);
        }

        public async Task DeletePaymentAsync(int id)
        {
            await _paymentRepository.DeleteAsync(id);
        }

        #endregion
    }
}
