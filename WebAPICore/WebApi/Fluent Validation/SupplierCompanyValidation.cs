using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Fluent_Validation
{
    public class SupplierCompanyValidation : AbstractValidator<SupplierCompany>
    {
        private readonly AppDbContext _db;

        public SupplierCompanyValidation(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Name).NotEmpty().Must((model, Name) => BeUniqueName(model, Name)).WithMessage("A Supplier Company must have a name that is unique in the database");
            RuleFor(x => x.Email).NotEmpty().Must((model, Email) => BeUniqueEmail(model, Email)).WithMessage("A Supplier Company must have an email that is unique in the database");
            RuleFor(x => x.Token).NotEmpty();
        }


        private bool BeUniqueName(SupplierCompany supplierCompany, string name)
        {
            var dbsupplierCompany = _db.SupplierCompanies.Where(x => x.Name == name).FirstOrDefault();
            if (dbsupplierCompany == null)
                return true;
            return dbsupplierCompany.Id == supplierCompany.Id;
        }
        private bool BeUniqueEmail(SupplierCompany supplierCompany, string Email)
        {
            var dbsupplierCompany = _db.SupplierCompanies.Where(x => x.Email == Email).FirstOrDefault();
            if (dbsupplierCompany == null)
                return true;
            return dbsupplierCompany.Id == supplierCompany.Id;
        }

    }
}
