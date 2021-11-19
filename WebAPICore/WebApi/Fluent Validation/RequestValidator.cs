using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Fluent_Validation
{
    public class RequestValidator : AbstractValidator<Request>
    {
        private readonly AppDbContext _db;

        public RequestValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id);
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.No).NotEmpty();

            RuleFor(x => x.BusinessPartnerId).NotEmpty()
                .WithMessage("An Request's BusinessPartnerId is required in the database")
                .Must(BeRegisteredBusinessPartner)
                .WithMessage("A Request's BusinessPartner should be reqistered in the database");

        }



        private bool BeRegisteredBusinessPartner(int id)
        {
            if (_db.BusinessPartners.Any(x => x.Id == id))
                return true;
            return false;
        }


    }
}
