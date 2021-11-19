using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class BusinessPartnerValidator : AbstractValidator<BusinessPartner>
    {
        private readonly AppDbContext _db;

        public BusinessPartnerValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Name).NotEmpty().Must((model, Name) => BeUniqueName(model, Name)).WithMessage("A Business Partner must have a name that is unique in the database");
            RuleFor(x => x.Email).NotEmpty().Must((model, Email) => BeUniqueEmail(model, Email)).WithMessage("A Business Partner must have an email that is unique in the database");
        }


        private bool BeUniqueName(BusinessPartner businessPartner, string name)
        {
            var dbbusinessPartner = _db.BusinessPartners.Where(x => x.Name == name).FirstOrDefault();
            if (dbbusinessPartner == null)
                return true;
            return dbbusinessPartner.Id == businessPartner.Id;
        }
        private bool BeUniqueEmail(BusinessPartner businessPartner, string Email)
        {
            var dbbusinessPartner = _db.BusinessPartners.Where(x => x.Email == Email).FirstOrDefault();
            if (dbbusinessPartner == null)
                return true;
            return dbbusinessPartner.Id == businessPartner.Id;
        }
    }
}
