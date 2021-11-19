using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class ResponsiblePersonValidator : AbstractValidator<ResponsiblePerson>
    {
        private readonly AppDbContext _db;
        public ResponsiblePersonValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Name).NotEmpty().WithMessage("A Reponsible Person's Name is required in the database");
            RuleFor(x => x.SupplierCompanyId)
                .NotEmpty().WithMessage("A Reponsible Person's SupplierCompanyId is required in the database");
            RuleFor(x => x.SupplierCompanyId)
                .Must(BeRegisteredSupplierCompany).WithMessage("A Reponsible Person's SupplierCompaynId should be a reqistered SupplierCompany in the database");
        }

        private bool BeRegisteredSupplierCompany(int id)
        {
            if (_db.SupplierCompanies.Any(x=>x.Id == id))
                return true;
            return false;
        }
    }
}
