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
    public class ResponsiblePeopleController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponsiblePersonValidator _rpValidator;
        private readonly SCRPPhoneNumberValidator _phoneNumberValidator;
        private readonly SCRPEmailValidator _emailValidator;
        private readonly IMapper _mapper;

        public ResponsiblePeopleController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _rpValidator = new ResponsiblePersonValidator(_db);
            _phoneNumberValidator = new SCRPPhoneNumberValidator(_db);
            _emailValidator = new SCRPEmailValidator(_db);
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.SCResponsiblePeople.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.ResponsiblePeople.Count().ToString(),
                rp = _mapper.Map<List<ResponsiblePersonDto>>(await _db.ResponsiblePeople.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.SCResponsiblePeople.Create)]
        public async Task<ApiResponse> Create([FromBody] ResponsiblePersonDto responsiblePerson)
        {
            ResponsiblePerson _responsiblePersonModel = _mapper.Map<ResponsiblePerson>(responsiblePerson);
            ValidationResult result = _rpValidator.Validate(_responsiblePersonModel);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.ResponsiblePeople.AddAsync(_responsiblePersonModel);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<ResponsiblePersonDto>(_responsiblePersonModel), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.SCResponsiblePeople.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<ResponsiblePersonDto>(responsiblePerson));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.SCResponsiblePeople.Edit)]
        public async Task<ApiResponse> Put(int id, ResponsiblePersonDto responsiblePerson)
        {
            ResponsiblePerson existingResponsiblePerson = await _db.ResponsiblePeople.FindAsync(id);
            if (existingResponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingResponsiblePerson.Name = responsiblePerson.Name;
            existingResponsiblePerson.Position = responsiblePerson.Position;

            ValidationResult result = _rpValidator.Validate(existingResponsiblePerson);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

           
            _db.Update(existingResponsiblePerson);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.SCResponsiblePeople.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var responsiblePerson = await _db.ResponsiblePeople.FindAsync(id);
            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.ResponsiblePeople.Remove(responsiblePerson);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion

        #region ResponsiblePerson Contact Operations

        [HttpGet]
        [Authorize(Permission.SCResponsiblePeople.View)]
        [Route("/api/responsiblepeople/contact")]
        public async Task<ApiResponse> GetWithContact([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.ResponsiblePeople.Count().ToString(),
                rpcontact = _mapper.Map<List<ResponsiblePersonWithContactDto>>(await _db.ResponsiblePeople.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }


        [HttpGet]
        [Authorize(Permission.SCResponsiblePeople.View)]
        [Route("/api/responsiblepeople/{id}/contact")]
        public async Task<ApiResponse> GetByIdWithContact([FromRoute] int id)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.FindAsync(id);
            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<ResponsiblePersonWithContactDto>(responsiblePerson));
        }

        #endregion

        #region Responsible Person Phone Number Operations

        [HttpGet]
        [Authorize(Permission.SCResponsiblePeople.View)]
        [Route("/api/responsiblepeople/{rpid}/phonenumbers")]
        public async Task<ApiResponse> GetWithPhoneNumbers(int rpid, [FromQuery] QueryParams qp)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.FindAsync(rpid);
            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);
            int count = responsiblePerson.PhoneNumbers.Count();
            responsiblePerson.PhoneNumbers = responsiblePerson.PhoneNumbers.AsQueryable().Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToList();

            object response = new
            {
                count = count.ToString(),
                rpphonenumbers = _mapper.Map<ResponsiblePersonWithPhoneNumberDto>(responsiblePerson)
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.SCRPPhones.Create)]
        [Route("/api/responsiblepeople/{rpid}/phonenumbers")]
        public async Task<ApiResponse> CreatePhoneNumber(int rpid, SCRPPhoneNumberDto gsm)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.Where(x => x.Id == rpid).FirstOrDefaultAsync();
            SCRPPhoneNumber sCRPPhoneNumber = _mapper.Map<SCRPPhoneNumber>(gsm);
            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);
            try
            {
                ValidationResult result = _phoneNumberValidator.Validate(sCRPPhoneNumber);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                    }
                    throw new ApiProblemDetailsException(ModelState);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
           

            _db.SCRPPhoneNumbers.Add(sCRPPhoneNumber);
            await _db.SaveChangesAsync();

            return new ApiResponse("Phone number added to responsible person succesfully.", _mapper.Map<SCRPPhoneNumberDto>(sCRPPhoneNumber), 201);
        }

        [HttpPut]
        [Authorize(Permission.SCRPPhones.Edit)]
        [Route("/api/responsiblepeople/{rpid}/phonenumbers/{phid}")]
        public async Task<ApiResponse> UpdatePhoneNumber(int rpid, int phid, SCRPPhoneNumberDto gsm)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.Where(x => x.Id == rpid).FirstOrDefaultAsync();
            SCRPPhoneNumber existingScrpPhoneNumber = await _db.SCRPPhoneNumbers.FindAsync(phid);

            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);

            if (existingScrpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {phid} does not exist.", 404);

            if(!responsiblePerson.PhoneNumbers.Any(x=>x.Id == phid))
                throw new ApiProblemDetailsException($"Responsinle with id: {rpid} doesn't has this phone number", 409);

            existingScrpPhoneNumber.Gsm = gsm.Gsm;

            ValidationResult result = _phoneNumberValidator.Validate(existingScrpPhoneNumber);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            
            _db.Update(existingScrpPhoneNumber);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [Authorize(Permission.SCRPPhones.Delete)]
        [HttpDelete]
        [Route("/api/responsiblepeople/{rpid}/phonenumbers/{phid}")]
        public async Task<ApiResponse> DeletePhoneNumber(int rpid, int phid)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.Where(x => x.Id == rpid).FirstOrDefaultAsync();
            SCRPPhoneNumber existingScrpPhoneNumber = await _db.SCRPPhoneNumbers.FindAsync(phid);

            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);

            if (existingScrpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {phid} does not exist.", 404);

            if (!responsiblePerson.PhoneNumbers.Any(x => x.Id == phid))
                throw new ApiProblemDetailsException($"ResponsiblePerson with id: {rpid} doesn't has this phone number", 409);

            _db.SCRPPhoneNumbers.Remove(existingScrpPhoneNumber);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion

        #region Responsible Person Email Operations

        [HttpGet]
        [Authorize(Permission.SCResponsiblePeople.View)]
        [Route("/api/responsiblepeople/{rpid}/emails")]
        public async Task<ApiResponse> GetWithEmails(int rpid, [FromQuery] QueryParams qp)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.FindAsync(rpid);
            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);

            int count = responsiblePerson.Emails.Count();
            responsiblePerson.Emails = responsiblePerson.Emails.AsQueryable().Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToList();
            object response = new
            {
                count = count.ToString(),
                rpemails = _mapper.Map<ResponsiblePersonWithEmailDto>(responsiblePerson)
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.SCRPEmails.Create)]
        [Route("/api/responsiblepeople/{rpid}/emails")]
        public async Task<ApiResponse> CreateEmail(int rpid, SCRPEmailDto email)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.Where(x => x.Id == rpid).FirstOrDefaultAsync();
            SCRPEmail sCRPEmail = _mapper.Map<SCRPEmail>(email);

            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);

            ValidationResult result = _emailValidator.Validate(sCRPEmail);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            _db.SCRPEmails.Add(sCRPEmail);
            await _db.SaveChangesAsync();

            return new ApiResponse("Phone number added to responsible person succesfully.", _mapper.Map<SCRPEmailDto>(sCRPEmail),201);
        }

        [HttpPut]
        [Authorize(Permission.SCRPEmails.Edit)]
        [Route("/api/responsiblepeople/{rpid}/emails/{eid}")]
        public async Task<ApiResponse> UpdateEmail(int rpid, int eid, SCRPEmailDto email)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.Where(x => x.Id == rpid).FirstOrDefaultAsync();
            SCRPEmail existingEmail = await _db.SCRPEmails.FindAsync(eid);

            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);

            if (existingEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {eid} does not exist.", 404);

            if (!responsiblePerson.Emails.Any(x => x.Id == eid))
                throw new ApiProblemDetailsException($"Responsinle with id: {rpid} doesn't has this email", 409);

            existingEmail.Email = email.Email;

            ValidationResult result = _emailValidator.Validate(existingEmail);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            
            _db.Update(existingEmail);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete]
        [Authorize(Permission.SCRPEmails.Delete)]
        [Route("/api/responsiblepeople/{rpid}/emails/{eid}")]
        public async Task<ApiResponse> DeleteEmail(int rpid, int eid)
        {
            ResponsiblePerson responsiblePerson = await _db.ResponsiblePeople.Where(x => x.Id == rpid).FirstOrDefaultAsync();
            SCRPEmail existingEmail = await _db.SCRPEmails.FindAsync(eid);

            if (responsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {rpid} does not exist.", 404);

            if (existingEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {eid} does not exist.", 404);

            if (!responsiblePerson.Emails.Any(x => x.Id == eid))
                throw new ApiProblemDetailsException($"ResponsiblePerson with id: {rpid} doesn't has this email", 409);

            _db.SCRPEmails.Remove(existingEmail);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
