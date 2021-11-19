using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Filters
{
    public class APIKeyAuthFilterAttribute : Attribute, IAuthorizationFilter
    {
        private const string _ApiKeyHeader = "x-api-key";
        private const string _ClientIdHeader = "x-client-id";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(_ApiKeyHeader, out var clientApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (!context.HttpContext.Request.Headers.TryGetValue(_ClientIdHeader, out var clientId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }


            var config = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var apiKey = config.GetValue<string>($"ApiKey:{clientId}");

            if(apiKey != clientApiKey)
                context.Result = new UnauthorizedResult();
        }
    }
}
