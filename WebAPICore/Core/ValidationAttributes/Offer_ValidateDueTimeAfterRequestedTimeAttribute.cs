using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValidationAttributes
{
    class Offer_ValidateDueTimeAfterRequestedTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //demonstration purpose
            //var ticket = validationContext.ObjectInstance as Ticket;
            //if (!ticket.ValidateDueTimeAfterBuyedTime())
            //    return new ValidationResult("Due date has to be in the future than BuyedTime");
            return ValidationResult.Success;
        }
    }
}
