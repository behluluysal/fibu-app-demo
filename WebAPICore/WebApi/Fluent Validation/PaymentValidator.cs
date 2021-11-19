using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Fluent_Validation
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        private readonly AppDbContext _db;
        public PaymentValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Price).NotEmpty().WithMessage("An Payment's Price is required");
            RuleFor(x => x.Date).NotEmpty().WithMessage("An Payment's Date is required");
            RuleFor(x => x.Date).LessThan(x => DateTime.Now).WithMessage("Please enter a valid payment date");
            RuleFor(x => x.Method).NotEmpty().WithMessage("An Payment's method is required");
        }

    }
}
