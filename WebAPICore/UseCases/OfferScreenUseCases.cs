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
    public class OfferScreenUseCases : IOfferScreenUseCases
    {
        private readonly IOfferRepository _offerRepository;
        public OfferScreenUseCases(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }



        #region CRUD Methods

        public async Task<(int, IEnumerable<OfferWithSupplierCompanyDto>)> ViewOffersAsync(QueryParams qp)
        {
            return await _offerRepository.GetAsync(qp);
        }
        public async Task<(int, IEnumerable<OfferWithSupplierCompanyDto>)> ViewOffersOfRequestedProductAsync(QueryParams qp, int requestedProductId)
        {
            qp.Filter += $" and (x=>x.RequestedProductId == \"{requestedProductId}\")";
            return await _offerRepository.GetAsync(qp);
        }

        public async Task<OfferWithSupplierCompanyDto> GetApprovedOffer(QueryParams qp, int requestedProductId)
        {
            qp.Filter += $" and (x=>x.isConfirmedOffer == \"True\") and (x=>x.RequestedProductId == \"{requestedProductId}\")";
            var response = await _offerRepository.GetAsync(qp);
            IEnumerable<OfferWithSupplierCompanyDto> offer = response.Item2;
            if (offer.Count() != 0)
                return offer.First();
            else
                return null;
        }

        public async Task<OfferWithSupplierCompanyDto> CreateOfferAsync(OfferCreateDto company)
        {
            return await _offerRepository.CreateAsync(company);
        }

        public async Task<OfferWithSupplierCompanyDto> ViewOfferByIdAsync(int id)
        {
            return await _offerRepository.GetByIdAsync(id);
        }

        public async Task UpdateOfferAsync(int id, OfferWithSupplierCompanyDto company)
        {
            await _offerRepository.UpdateAsync(id, company);
        }

        public async Task DeleteOfferAsync(int id)
        {
            await _offerRepository.DeleteAsync(id);
        }

        #endregion
    }
}
