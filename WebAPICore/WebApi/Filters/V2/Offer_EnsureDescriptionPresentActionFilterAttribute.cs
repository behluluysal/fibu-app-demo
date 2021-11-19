using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Filters.V2
{
    public class Offer_EnsureDescriptionPresentActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Demonstration purpose
            //base.OnActionExecuting(context);
            //var offer = context.ActionArguments["offer"] as Offer;

            //if (offer != null && !offer.ValidateDescription())
            //{
            //    context.ModelState.AddModelError("Description", "Description is required");
            //    context.Result = new BadRequestObjectResult(context.ModelState);
            //}                           
        }
    }
}
