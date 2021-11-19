using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class BPRPPhoneNumberValidator : AbstractValidator<BPRPPhoneNumber>
    {
        private readonly AppDbContext _db;

        public BPRPPhoneNumberValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Gsm).NotEmpty().Must((model, Gsm) => BeUniquePhoneNumber(model, Gsm)).WithMessage("A gsm should be unique in the database");
            RuleFor(x => x.BPResponsiblePersonId).NotEmpty().WithMessage("A Phone number's ResponsiblePersonID is required in the database");
            RuleFor(x => x.BPResponsiblePersonId)
               .Must(BeRegisteredResponsiblePerson).WithMessage("A Phone number's ResponsiblePersonID should be a reqistered ResponsiblePerson in the database");
        }


        private bool BeUniquePhoneNumber(BPRPPhoneNumber phoneNumber, string gsm)
        {
            var dbPhoneNumber = _db.BPRPPhoneNumbers.Where(x => x.Gsm == gsm).FirstOrDefault();
            if (dbPhoneNumber == null)
                return true;
            return dbPhoneNumber.Id == phoneNumber.Id;
        }
        private bool BeRegisteredResponsiblePerson(int id)
        {
            if (_db.BPResponsiblePeople.Any(x => x.Id == id))
                return true;
            return false;
        }
    }
}
