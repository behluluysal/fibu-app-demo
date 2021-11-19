using Core.Models;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Auth;

namespace WebAPI.Utility
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        //IHttpContextAccessor _httpContextAccessor = null; //for token valid time check
        //private IMemoryCache _cache;

        public PermissionAuthorizationHandler(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            //_httpContextAccessor = httpContextAccessor; //for then valid time check also dont forget to add to the constructor parameter
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                #region Token Valid time Check (Invalid)
                //If you want to check token valid time you need to use httpContext and check for token validation end date
                //HttpContext httpContext = _httpContextAccessor.HttpContext;
                //var tokenManager = httpContext.RequestServices.GetService(typeof(ICustomTokenManager)) as ICustomTokenManager;
                //if (tokenManager != null && !tokenManager.VerifyToken(token))
                //{
                //    context.Result = new UnauthorizedResult();
                //    return;
                //}
                #endregion  

                string id = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                
                if(context.User.HasClaim(c=> c.Value == requirement.Permission))
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    context.Fail();
                    return;
                }
            }
            else
            {
                context.Fail();
                return;
            }
        }
    }
}
