using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Fluent_Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        private readonly AppDbContext _db;

        public ProductValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Name).NotEmpty().Must((model, Name) => BeUniqueName(model, Name)).WithMessage("A Product must have a name that is unique in the database");
        }


        private bool BeUniqueName(Product product, string name)
        {
            var dbProduct = _db.Products.Where(x => x.Name == name).FirstOrDefault();
            if (dbProduct == null)
                return true;
            return product.Id == dbProduct.Id;
        }
    }
}