using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebAPI.Fluent_Validation
{
    public class TagValidator : AbstractValidator<Tag>
    {
        private readonly AppDbContext _db;

        public TagValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty().Must( (model, Name)=> BeUniqueName(model,Name) ).WithMessage("A tag must have a name that is unique in the database");
        }


        private bool BeUniqueName(Tag tag, string name)
        {
            var dbTag = _db.Tags.Where(x => x.Name == name).FirstOrDefault();
            if (dbTag == null)
                return true;
            return dbTag.Id == tag.Id;
        }
    }
}
