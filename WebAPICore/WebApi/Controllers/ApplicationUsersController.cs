using AutoMapper;
using Core.AutoMapperDtos;
using Core.Mapper;
using Core.Models;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using AutoWrapper.Wrappers;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using RazorHtmlEmails.RazorClassLib.Models;
using WebAPI.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.NewAccount;
using System.Linq;
using WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Core.Utility;

namespace WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenValidateAttribute]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailSenderUseCases _emailSenderUseCases;
        
        public ApplicationUsersController(AppDbContext db, IMapper mapper, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<Role> roleManager,
            IEmailSenderUseCases emailSenderUseCases)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            this._emailSenderUseCases = emailSenderUseCases;
        }

        #region User Crud Operations
        [HttpGet]
        [Authorize(Permission.Users.View)]
        public async Task<ApiResponse> Get()
        {
            object response = new
            {
                count = _db.ApplicationUsers.Count().ToString(),
                applicationusers = _mapper.Map<List<ApplicationUserDto>>(await _db.ApplicationUsers.ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.Users.Create)]
        public async Task<ApiResponse> Create([FromBody] CreateUserViewModel applicationUser, [FromServices] IEmailSender emailSender)
        {
            ApplicationUser Nuser = new ApplicationUser
            {
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
            };
            
            IdentityResult result = await _userManager.CreateAsync(Nuser, applicationUser.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            try
            {
                await _emailSenderUseCases.CreateEmailSpecializedAsync(
                    applicationUser.Email, 
                    "Welcome to FIBU Online System", 
                    new NewAccountEmailViewModel(this.Request.Host.ToString(), applicationUser)
                    );
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(Nuser);
                throw new ApiProblemDetailsException("There was an error on Email sending.", 500);
            }

            return new ApiResponse("New record has been created in the database.", _mapper.Map<ApplicationUserDto>(Nuser), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.Users.View)]
        public async Task<ApiResponse> GetById([FromRoute] string id)
        {
            
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            return new ApiResponse(_mapper.Map<ApplicationUserDto>(applicationUser), 200);
        }

        [HttpPut("{id}")]
        [Authorize(Permission.Users.Edit)]
        public async Task<ApiResponse> Put(string id, CreateUserViewModel applicationUser)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            user.UserName = applicationUser.UserName;
            user.Email = applicationUser.Email;
            IdentityResult response= await _userManager.UpdateAsync(user);
            if(!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            
            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.Users.Delete)]
        public async Task<ApiResponse> Delete(string id)
        {
            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            await _userManager.DeleteAsync(applicationUser);

            return new ApiResponse(204);
        }
        #endregion

        #region User Role Crud Operations
        [HttpGet]
        [Authorize(Permission.Users.View)]
        [Route("/api/applicationusers/{uid}/roles")]
        public async Task<ApiResponse> GetRoles(string uid)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(uid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            IList<string> rolenames = await _userManager.GetRolesAsync(user);
            IList<RoleDto> roles = new List<RoleDto>();

            foreach (var item in rolenames)
            {
                roles.Add(_mapper.Map<RoleDto>(await _roleManager.FindByNameAsync(item)));
            }
            return new ApiResponse(_mapper.Map<ApplicationUserWithRoleDto>(user, opt => opt.Items["Roles"] = roles));
        }

        [HttpPost]
        [Authorize(Permission.Users.Edit)]
        [Route("/api/applicationusers/{uid}/roles/{rid}")]
        public async Task<ApiResponse> AssignRole(string uid, string rid)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(uid);
            Role role = await _roleManager.FindByIdAsync(rid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Assign the role from user
            IdentityResult response = await _userManager.AddToRoleAsync(user, role.Name);
            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            IList<string> rolenames = await _userManager.GetRolesAsync(user);

            IList<RoleDto> roles = new List<RoleDto>();

            foreach (var item in rolenames)
            {
                roles.Add(_mapper.Map<RoleDto>(await _roleManager.FindByNameAsync(item)));
            }

            return new ApiResponse("Role assigned to user succesfully.", _mapper.Map<ApplicationUserWithRoleDto>(user, opt => opt.Items["Roles"] = roles));
        }

        [HttpDelete]
        [Authorize(Permission.Users.Edit)]
        [Route("/api/applicationusers/{uid}/roles/{rid}")]
        public async Task<ApiResponse> WitdrawRole(string uid, string rid)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(uid);
            Role role = await _roleManager.FindByIdAsync(rid);
            if (user == null)
                throw new ApiProblemDetailsException($"Record with id: {uid} does not exist.", 404);
            if (role == null)
                throw new ApiProblemDetailsException($"Record with id: {rid} does not exist.", 404);

            //Withdraw the role from user
            IdentityResult response = await _userManager.RemoveFromRoleAsync(user, role.Name);

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

        #endregion
    }
}