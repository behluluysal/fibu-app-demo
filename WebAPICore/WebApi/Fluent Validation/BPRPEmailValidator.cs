using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class BPRPEmailValidator : AbstractValidator<BPRPEmail>
    {
        private readonly AppDbContext _db;

        public BPRPEmailValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id);
            RuleFor(x => x.Email).NotEmpty().Must((model, Email) => BeUniqueEmail(model, Email)).WithMessage("An email should be unique in the database");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email adress");
            RuleFor(x => x.BPResponsiblePersonId).NotEmpty().WithMessage("An Email's ResponsiblePersonID is required in the database");
            RuleFor(x => x.BPResponsiblePersonId)
              .Must(BeRegisteredResponsiblePerson).WithMessage("An Email's ResponsiblePersonID should be a reqistered ResponsiblePerson in the database");
            RuleFor(x => x.Email)
              .Must(BeUnregisteredUser).WithMessage("This email already has a user in the database.");
        }


        private bool BeUniqueEmail(BPRPEmail sCRPEmail, string email)
        {
            var dbEmail = _db.BPRPEmails.Where(x => x.Email == email).FirstOrDefault();
            if (dbEmail == null)
                return true;
            return sCRPEmail.Id == dbEmail.Id;
        }

        private bool BeUnregisteredUser(BPRPEmail sCRPEmail, string email)
        {
            if (sCRPEmail.CanLogin == false)
                return true;

            var dbUser = _db.ApplicationUsers.Where(x => x.Email == email).FirstOrDefault();
            if (dbUser == null)
                return true;
            return false;
        }

        private bool BeRegisteredResponsiblePerson(int id)
        {
            if (_db.BPResponsiblePeople.Any(x => x.Id == id))
                return true;
            return false;
        }
    }
}
