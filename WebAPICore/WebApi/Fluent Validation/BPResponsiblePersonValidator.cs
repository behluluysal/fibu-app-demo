using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class BPResponsiblePersonValidator : AbstractValidator<BPResponsiblePerson>
    {
        private readonly AppDbContext _db;
        public BPResponsiblePersonValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Name).NotEmpty().WithMessage("A Reponsible Person's Name is required in the database");
            RuleFor(x => x.BusinessPartnerId)
                .NotEmpty().WithMessage("A Reponsible Person's BusinessPartnerId is required in the database");
            RuleFor(x => x.BusinessPartnerId)
                .Must(BeRegisteredSupplierCompany).WithMessage("A Reponsible Person's BusinessPartnerId should be a reqistered BusinessPartner in the database");
        }

        private bool BeRegisteredSupplierCompany(int id)
        {
            if (_db.BusinessPartners.Any(x => x.Id == id))
                return true;
            return false;
        }
    }
}
