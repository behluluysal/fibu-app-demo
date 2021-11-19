using AutoMapper;
using AutoWrapper.Wrappers;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.Utility;
using DataStore.EF.Data;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
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
    public class SupplierCompaniesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly SupplierCompanyValidation _companyValidator;
        private readonly IMapper _mapper;
        public SupplierCompaniesController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _companyValidator = new SupplierCompanyValidation(_db);
        }

        #region Crud Operations

        [HttpGet]
        [Authorize(Permission.SupplierCompanies.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.SupplierCompanies.Count().ToString(),
                companies = _mapper.Map<List<SupplierCompanyDto>>(await _db.SupplierCompanies.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.SupplierCompanies.Create)]
        public async Task<ApiResponse> Create([FromBody] SupplierCompanyDto company)
        {
            SupplierCompany newCompany = _mapper.Map<SupplierCompany>(company);
            newCompany.Token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            ValidationResult result = _companyValidator.Validate(newCompany);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            
            await _db.SupplierCompanies.AddAsync(newCompany);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<SupplierCompanyDto>(newCompany), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.SupplierCompanies.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            SupplierCompany company = await _db.SupplierCompanies.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (company == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<SupplierCompanyDto>(company));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.SupplierCompanies.Edit)]
        public async Task<ApiResponse> Put(int id, SupplierCompanyDto company)
        {
            SupplierCompany existingCompany = await _db.SupplierCompanies.FindAsync(id);
            if (existingCompany == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingCompany.Name = company.Name;
            existingCompany.Email = company.Email;
            existingCompany.Gsm = company.Adress;
            existingCompany.Adress = company.Adress;
            existingCompany.Phone = company.Phone;

            ValidationResult result = _companyValidator.Validate(existingCompany);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            _db.Update(existingCompany);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.SupplierCompanies.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var company = await _db.SupplierCompanies.FindAsync(id);
            if (company == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.SupplierCompanies.Remove(company);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }
        #endregion

        #region Supplier Company Tag Operations

        [HttpGet]
        [Authorize(Permission.SupplierCompanies.View)]
        [Route("/api/suppliercompanies/{sid}/tags")]
        public async Task<ApiResponse> GetTags(int sid)
        {
            SupplierCompany company = await _db.SupplierCompanies.FindAsync(sid);
            if (company == null)
                throw new ApiProblemDetailsException($"Record with id: {sid} does not exist.", 404);
            return new ApiResponse(_mapper.Map<SupplierCompanyWithTagDto>(company));
        }

        [HttpPost]
        [Authorize(Permission.SupplierCompanies.EditTag)]
        [Route("/api/suppliercompanies/{sid}/tags/{tid}")]
        public async Task<ApiResponse> AssignTag(int sid, int tid)
        {
            SupplierCompany company = await _db.SupplierCompanies.Where(x => x.Id == sid).FirstOrDefaultAsync();
            Tag tag = await _db.Tags.Where(x => x.Id == tid).FirstOrDefaultAsync();

            if (company == null)
                throw new ApiProblemDetailsException($"Record with id: {sid} does not exist.", 404);
            if (tag == null)
                throw new ApiProblemDetailsException($"Record with id: {tid} does not exist.", 404);

            if (company.SupplierCompanyTags.Any(x => x.Tag == tag))
                throw new ApiProblemDetailsException($"SupplierCompany with id: {sid} already has this tag", 409);

            SupplierCompanyTag companyTag = new SupplierCompanyTag { SupplierCompany = company, SupplierCompanyId = company.Id, Tag = tag, TagId = tag.Id };
            company.SupplierCompanyTags.Add(companyTag);
            tag.SupplierCompanyTags.Add(companyTag);
            await _db.SaveChangesAsync();

            return new ApiResponse("Tag assigned to company succesfully.", _mapper.Map<SupplierCompanyWithTagDto>(company));
        }

        [HttpDelete]
        [Authorize(Permission.SupplierCompanies.EditTag)]
        [Route("/api/suppliercompanies/{sid}/tags/{tid}")]
        public async Task<ApiResponse> WithdrawTag(int sid, int tid)
        {
            SupplierCompany company = await _db.SupplierCompanies.Where(x => x.Id == sid).FirstOrDefaultAsync();
            Tag tag = await _db.Tags.Where(x => x.Id == tid).FirstOrDefaultAsync();

            if (company == null)
                throw new ApiProblemDetailsException($"Record with id: {sid} does not exist.", 404);
            if (tag == null)
                throw new ApiProblemDetailsException($"Record with id: {tid} does not exist.", 404);

            if (!company.SupplierCompanyTags.Any(x => x.Tag == tag))
                throw new ApiProblemDetailsException($"SupplierCompany with id: {sid} doesn't has this tag", 409);

            SupplierCompanyTag companyTag = await _db.SupplierCompanyTags.Where(x => x.Tag == tag && x.SupplierCompany == company).FirstOrDefaultAsync();
            _db.SupplierCompanyTags.Remove(companyTag);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        #endregion

        #region Supplier Company Responsible Person Operations

        [HttpGet]
        [Authorize(Permission.SupplierCompanies.View)]
        [Route("/api/suppliercompanies/{sid}/responsiblepeople")]
        public async Task<ApiResponse> GetResponsiblePeople(int sid, [FromQuery]QueryParams qp)
        {
            SupplierCompany company = await _db.SupplierCompanies.FindAsync(sid);
            if (company == null)
                throw new ApiProblemDetailsException($"Record with id: {sid} does not exist.", 404);
            company.ResponsiblePeople = company.ResponsiblePeople.AsQueryable().Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToList();
            return new ApiResponse(_mapper.Map<SupplierCompanyWithResponsiblePersonDto>(company));
        }

        #endregion


    }
}
