using AutoMapper;
using AutoWrapper.Wrappers;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.Utility;
using DataStore.EF.Data;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Fluent_Validation;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidateAttribute]
    public class BusinessPartnersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly BusinessPartnerValidator _businessPartnerValidator;
        private readonly IMapper _mapper;

        public BusinessPartnersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _businessPartnerValidator = new BusinessPartnerValidator(_db);
        }

        #region Crud Operations

        [HttpGet]
        [Authorize(Permission.BusinessPartners.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.BusinessPartners.Count().ToString(),
                businesspartners = _mapper.Map<List<BusinessPartnerDto>>(await _db.BusinessPartners.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.BusinessPartners.Create)]
        public async Task<ApiResponse> Create([FromBody] BusinessPartner businessPartner)
        {
            ValidationResult result = _businessPartnerValidator.Validate(businessPartner);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.BusinessPartners.AddAsync(businessPartner);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<BusinessPartnerDto>(businessPartner), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.BusinessPartners.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            BusinessPartner businessPartner = await _db.BusinessPartners.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (businessPartner == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<BusinessPartnerDto>(businessPartner));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.BusinessPartners.Edit)]
        public async Task<ApiResponse> Put(int id, BusinessPartnerDto businessPartner)
        {
            BusinessPartner existingBusinessPartner = await _db.BusinessPartners.FindAsync(id);
            if (existingBusinessPartner == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingBusinessPartner.Name = businessPartner.Name;
            existingBusinessPartner.Email = businessPartner.Email;
            existingBusinessPartner.Gsm = businessPartner.Adress;
            existingBusinessPartner.Adress = businessPartner.Adress;
            existingBusinessPartner.Phone = businessPartner.Phone;

            ValidationResult result = _businessPartnerValidator.Validate(existingBusinessPartner);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            _db.Update(existingBusinessPartner);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.BusinessPartners.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var businessPartner = await _db.BusinessPartners.FindAsync(id);
            if (businessPartner == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.BusinessPartners.Remove(businessPartner);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }
        #endregion


        #region Business Partner Responsible Person Operations

        [HttpGet]
        [Authorize(Permission.BusinessPartners.View)]
        [Route("/api/businesspartners/{bpid}/responsiblepeople")]
        public async Task<ApiResponse> GetResponsiblePeople(int bpid, [FromQuery] QueryParams qp)
        {
            BusinessPartner businessPartner = await _db.BusinessPartners.FindAsync(bpid);
            if (businessPartner == null)
                throw new ApiProblemDetailsException($"Record with id: {bpid} does not exist.", 404);
            int count = businessPartner.BPResponsiblePeople.Count();
            businessPartner.BPResponsiblePeople = businessPartner.BPResponsiblePeople.AsQueryable().Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToList();

            object response = new
            {
                count = count.ToString(),
                businesspartner = _mapper.Map<BusinessPartnerWithResponsiblePersonDto>(businessPartner)
            };
            return new ApiResponse(response);
        }

        #endregion


        #region Business Partner Requests Operations

        [HttpGet]
        [Authorize(Permission.BusinessPartners.View)]
        [Route("/api/businesspartners/{bpid}/requests")]
        public async Task<ApiResponse> GetRequests(int bpid, [FromQuery] QueryParams qp)
        {
            BusinessPartner businessPartner = await _db.BusinessPartners.FindAsync(bpid);
            if (businessPartner == null)
                throw new ApiProblemDetailsException($"Record with id: {bpid} does not exist.", 404);
            businessPartner.Requests = businessPartner.Requests.AsQueryable().Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToList();
            int count = businessPartner.Requests.Count();
            object response = new
            {
                count = count.ToString(),
                businesspartner = _mapper.Map<BusinessPartnerWithRequestsDto>(businessPartner)
            };
            return new ApiResponse(response);
        }

        [HttpGet]
        [Authorize(Permission.BusinessPartners.View)]
        [Route("/api/businesspartners/{bpid}/requests/{rid}")]
        public async Task<ApiResponse> GetRequestWithRequestedProducts(int bpid,int rid)
        {
            BusinessPartner businessPartner = await _db.BusinessPartners.FindAsync(bpid);
            if (businessPartner == null)
                throw new ApiProblemDetailsException($"Record with id: {bpid} does not exist.", 404);
            BusinessPartnerWithRequestWithRequestedProductsDto ResponseObject = _mapper.Map<BusinessPartnerWithRequestWithRequestedProductsDto>(businessPartner);

            //mustang, try to change to async
            ResponseObject.Request = _mapper.Map<RequestWithRequestedProductDto>(businessPartner.Requests.AsQueryable().FirstOrDefault(x => x.Id == rid));
            return new ApiResponse(ResponseObject);
        }

        #endregion
    }
}
