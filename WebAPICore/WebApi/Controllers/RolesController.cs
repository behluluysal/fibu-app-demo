using AutoMapper;
using Core.AutoMapperDtos;
using Core.Models;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Core.Utility;
using AutoWrapper.Wrappers;
using Core.Pagination;
using System.Linq.Dynamic.Core;
using WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenValidateAttribute]
    public class RolesController : ControllerBase
    {
        
        private readonly AppDbContext _db;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(AppDbContext db, IMapper mapper, RoleManager<Role> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        #region Role Crud Operations

        [HttpGet]
        [Authorize(Permission.Roles.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.Roles.Count().ToString(),
                roles = _mapper.Map<List<RoleDto>>(await _roleManager.Roles.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.Roles.Create)]
        public async Task<ApiResponse> Create([FromBody] RoleDto role)
        {
            Role rol = new Role();
            rol.Name = role.Name;
            IdentityResult result = await _roleManager.CreateAsync(rol);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            return new ApiResponse(_mapper.Map<RoleDto>(role),201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.Roles.View)]
        public async Task<ApiResponse> GetById([FromRoute] string id)
        {
            Role role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            return new ApiResponse(_mapper.Map<RoleDto>(role));
        }

        [HttpPut("{id}")]
        [Authorize(Permission.Roles.Edit)]
        public async Task<ApiResponse> Put(string id, RoleDto Requestrole)
        {
            Role role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            role.Name = Requestrole.Name;
            await _roleManager.UpdateAsync(role);

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.Roles.Delete)]
        public async Task<ApiResponse> Delete(string id)
        {
            Role role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            await _roleManager.DeleteAsync(role);

            return new ApiResponse(204);
        }
        #endregion

        #region Role Claim Operations
        [HttpGet]
        [Route("/api/roles/{rid}/claims")]
        public async Task<ApiResponse> GetClaims(string rid)
        {
            Role role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);
            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));
            return new ApiResponse(_mapper.Map<RoleWithClaimDto>(role, opt => opt.Items["Claims"] = claims));
        }

        [HttpPost]
        [Authorize(Permission.Roles.AssignPermission)]
        [Route("/api/roles/{rid}/claims")]
        public async Task<ApiResponse> AssignClaim(string rid, [FromBody] ClaimDto claim)
        {
            Role role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign claim from role
            IList<ClaimDto> claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));

            //Manual check if role has this claim
            if (claims.Any(x => x.Value == claim.Value && x.Type == claim.Type))
                throw new ApiProblemDetailsException($"Role with id {rid} already has this claim", 409);

            IdentityResult response = await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));

            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            //Refill claims list to send it as response
            claims = _mapper.Map<List<ClaimDto>>(await _roleManager.GetClaimsAsync(role));
            return new ApiResponse(_mapper.Map<RoleWithClaimDto>(role, opt => opt.Items["Claims"] = claims));
        }

        [HttpDelete]
        [Authorize(Permission.Roles.WithdrawPermission)]
        [Route("/api/roles/{rid}/claims/{claimValue}")]
        public async Task<ApiResponse> WithdrawClaim(string rid, string claimValue)
        {
            Role role = await _roleManager.FindByIdAsync(rid);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign claim from role
            IList<Claim> claims = await _roleManager.GetClaimsAsync(role);

            IdentityResult response = await _roleManager.RemoveClaimAsync(role, new Claim(CustomClaimTypes.APIPermission, claimValue));

            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            return new ApiResponse(204);
        }

        /*  Works with body object, will be changed to url "Claim Value" because we don't have claim in db means no id we have.
       [HttpDelete]
       [Route("/api/roles/{rid}/claims")]
       public async Task<IActionResult> WithdrawClaim(string rid, [FromBody] AssignClaimDto claim)
       {
           try
           {
               Role role = await _roleManager.FindByIdAsync(rid);
               if (role == null) return NotFound();

               //Assign claim from role
               IList<Claim> claims = await _roleManager.GetClaimsAsync(role);

               IdentityResult response = await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType, claim.ClaimValue));

               if (!response.Succeeded)
               {
                   foreach (var error in response.Errors)
                   {
                       ModelState.AddModelError(error.Code, error.Description);
                   }
                   return BadRequest(ModelState);
               }
               return NoContent();
           }
           catch (Exception e)
           {
               return BadRequest(e.Message);
           }
       }
       */
        #endregion

    }
}