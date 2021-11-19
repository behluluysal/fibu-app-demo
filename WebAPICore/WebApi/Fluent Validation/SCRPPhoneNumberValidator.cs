using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class SCRPPhoneNumberValidator : AbstractValidator<SCRPPhoneNumber>
    {
        private readonly AppDbContext _db;

        public SCRPPhoneNumberValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Gsm).NotEmpty().Must((model, Gsm) => BeUniquePhoneNumber(model, Gsm)).WithMessage("A gsm should be unique in the database");
            RuleFor(x => x.ResponsiblePersonId).NotEmpty().WithMessage("A Phone number's ResponsiblePersonID is required in the database");
            RuleFor(x => x.ResponsiblePersonId)
               .Must(BeRegisteredResponsiblePerson).WithMessage("A Phone number's ResponsiblePersonID should be a reqistered ResponsiblePerson in the database");
        }


        private bool BeUniquePhoneNumber(SCRPPhoneNumber phoneNumber, string gsm)
        {
            var dbPhoneNumber = _db.SCRPPhoneNumbers.Where(x => x.Gsm == gsm).FirstOrDefault();
            if (dbPhoneNumber == null)
                return true;
            return dbPhoneNumber.Id == phoneNumber.Id;
        }
        private bool BeRegisteredResponsiblePerson(int id)
        {
            if (_db.ResponsiblePeople.Any(x => x.Id == id))
                return true;
            return false;
        }
    }
}
