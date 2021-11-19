using Core.Models;
using DataStore.EF.Data;
using FluentValidation;
using System.Linq;

namespace WebAPI.Fluent_Validation
{
    public class RequestedProductValidator : AbstractValidator<RequestedProduct>
    {
        private readonly AppDbContext _db;

        public RequestedProductValidator(AppDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id);
            RuleFor(x => x.ProductId).NotEmpty()
                .WithMessage("A Request's Product is required in the database")
                .Must(BeRegisteredProduct)
                .WithMessage("A RequestedProduct's Product should be reqistered in the database");


            RuleFor(x => x.RequestId).NotEmpty()
                .WithMessage("An Request's BusinessPartnerId is required in the database")
                .Must(BeRegisteredRequest)
                .WithMessage("A RequestedProduct's Request should be reqistered in the database");

        }
        private bool BeRegisteredProduct(int id)
        {
            if (_db.Products.Any(x => x.Id == id))
                return true;
            return false;
        }
        private bool BeRegisteredRequest(int id)
        {
            if (_db.Requests.Any(x => x.Id == id))
                return true;
            return false;
        }
    }
}
