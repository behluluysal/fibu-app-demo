using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Fluent_Validation
{
    public class OfferValidator : AbstractValidator<Offer>
    {
        private readonly AppDbContext _db;
        public OfferValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.SupplierCompanyId)
                .NotEmpty().WithMessage("An Offer's SupplierCompany is required in the database");
            RuleFor(x => x.SupplierCompanyId)
                .Must(BeRegisteredSupplierCompany).WithMessage("An Offer's SupplierCompanyId should be a reqistered SupplierCompany in the database");

            RuleFor(x => x.RequestedProductId)
               .NotEmpty().WithMessage("An Offer's RequestedProductId is required in the database");
            RuleFor(x => x.RequestedProductId)
                .Must(BeRegisteredRequestedProduct).WithMessage("An Offer's RequestedProductId should be a reqistered RequestedProduct in the database");
        }

        private bool BeRegisteredSupplierCompany(int id)
        {
            if (_db.SupplierCompanies.Any(x => x.Id == id))
                return true;
            return false;
        }

        private bool BeRegisteredRequestedProduct(int id)
        {
            if (_db.RequestedProducts.Any(x => x.Id == id))
                return true;
            return false;
        }
    }
}
