using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class SCRPEmailValidator : AbstractValidator<SCRPEmail>
    {
        private readonly AppDbContext _db;

        public SCRPEmailValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id);
            RuleFor(x => x.Email).NotEmpty().Must((model, Email) => BeUniqueEmail(model, Email)).WithMessage("An email should be unique in the database");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email adress");
            RuleFor(x => x.ResponsiblePersonId).NotEmpty().WithMessage("An Email's ResponsiblePersonID is required in the database");
            RuleFor(x => x.ResponsiblePersonId)
              .Must(BeRegisteredResponsiblePerson).WithMessage("An Email's ResponsiblePersonID should be a reqistered ResponsiblePerson in the database");
        }


        private bool BeUniqueEmail(SCRPEmail sCRPEmail, string email)
        {
            var dbEmail = _db.SCRPEmails.Where(x => x.Email == email).FirstOrDefault();
            if (dbEmail == null)
                return true;
            return sCRPEmail.Id == dbEmail.Id;
        }
        private bool BeRegisteredResponsiblePerson(int id)
        {
            if (_db.ResponsiblePeople.Any(x => x.Id == id))
                return true;
            return false;
        }
    }
}
