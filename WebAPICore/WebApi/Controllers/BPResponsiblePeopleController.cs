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
    public class BPResponsiblePeopleController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly BPResponsiblePersonValidator _bprpValidator;
        private readonly BPRPPhoneNumberValidator _phoneNumberValidator;
        private readonly BPRPEmailValidator _emailValidator;
        private readonly IMapper _mapper;

        public BPResponsiblePeopleController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _bprpValidator = new BPResponsiblePersonValidator(_db);
            _phoneNumberValidator = new BPRPPhoneNumberValidator(_db);
            _emailValidator = new BPRPEmailValidator(_db);
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.BPResponsiblePeople.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.BPResponsiblePeople.Count().ToString(),
                bprp = _mapper.Map<List<BPResponsiblePersonDto>>(await _db.BPResponsiblePeople.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.BPResponsiblePeople.Create)]
        public async Task<ApiResponse> Create([FromBody] BPResponsiblePersonDto bpresponsiblePerson)
        {
            BPResponsiblePerson _bpresponsiblePersonModel = _mapper.Map<BPResponsiblePerson>(bpresponsiblePerson);
            ValidationResult result = _bprpValidator.Validate(_bpresponsiblePersonModel);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.BPResponsiblePeople.AddAsync(_bpresponsiblePersonModel);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<BPResponsiblePersonDto>(_bpresponsiblePersonModel), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.BPResponsiblePeople.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<BPResponsiblePersonDto>(bpresponsiblePerson));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.BPResponsiblePeople.Edit)]
        public async Task<ApiResponse> Put(int id, BPResponsiblePersonDto bpresponsiblePerson)
        {
            BPResponsiblePerson existingBPResponsiblePerson = await _db.BPResponsiblePeople.FindAsync(id);
            if (existingBPResponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingBPResponsiblePerson.Name = bpresponsiblePerson.Name;
            existingBPResponsiblePerson.Position = bpresponsiblePerson.Position;

            ValidationResult result = _bprpValidator.Validate(existingBPResponsiblePerson);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }


            _db.Update(existingBPResponsiblePerson);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.BPResponsiblePeople.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var bpresponsiblePerson = await _db.BPResponsiblePeople.FindAsync(id);
            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.BPResponsiblePeople.Remove(bpresponsiblePerson);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion


        #region Responsible Person Phone Number Operations

        [HttpGet]
        [Authorize(Permission.BPRPPhones.View)]
        [Route("/api/bpresponsiblepeople/{bprpid}/phonenumbers")]
        public async Task<ApiResponse> GetWithPhoneNumbers(int bprpid, [FromQuery] QueryParams qp)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.FindAsync(bprpid);
            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);
            bpresponsiblePerson.PhoneNumbers = bpresponsiblePerson.PhoneNumbers.AsQueryable().Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToList();
            return new ApiResponse(_mapper.Map<BPResponsiblePersonWithPhoneNumberDto>(bpresponsiblePerson));
        }

        [HttpPost]
        [Authorize(Permission.BPRPPhones.Create)]
        [Route("/api/bpresponsiblepeople/{bprpid}/phonenumbers")]
        public async Task<ApiResponse> CreatePhoneNumber(int bprpid, BPRPPhoneNumberDto gsm)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.Where(x => x.Id == bprpid).FirstOrDefaultAsync();
            BPRPPhoneNumber bprpPhoneNumber = _mapper.Map<BPRPPhoneNumber>(gsm);
            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);
            try
            {
                ValidationResult result = _phoneNumberValidator.Validate(bprpPhoneNumber);
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


            _db.BPRPPhoneNumbers.Add(bprpPhoneNumber);
            await _db.SaveChangesAsync();

            return new ApiResponse("Phone number added to responsible person succesfully.", _mapper.Map<BPRPPhoneNumberDto>(bprpPhoneNumber), 201);
        }

        [HttpPut]
        [Authorize(Permission.BPRPPhones.Edit)]
        [Route("/api/bpresponsiblepeople/{bprpid}/phonenumbers/{phid}")]
        public async Task<ApiResponse> UpdatePhoneNumber(int bprpid, int phid, BPRPPhoneNumberDto gsm)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.Where(x => x.Id == bprpid).FirstOrDefaultAsync();
            BPRPPhoneNumber existingbprpPhoneNumber = await _db.BPRPPhoneNumbers.FindAsync(phid);

            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);

            if (existingbprpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {phid} does not exist.", 404);

            if (!bpresponsiblePerson.PhoneNumbers.Any(x => x.Id == phid))
                throw new ApiProblemDetailsException($"Responsinle with id: {bprpid} doesn't has this phone number", 409);

            existingbprpPhoneNumber.Gsm = gsm.Gsm;

            ValidationResult result = _phoneNumberValidator.Validate(existingbprpPhoneNumber);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }


            _db.Update(existingbprpPhoneNumber);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete]
        [Authorize(Permission.BPRPPhones.Delete)]
        [Route("/api/bpresponsiblepeople/{bprpid}/phonenumbers/{phid}")]
        public async Task<ApiResponse> DeletePhoneNumber(int bprpid, int phid)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.Where(x => x.Id == bprpid).FirstOrDefaultAsync();
            BPRPPhoneNumber existingbprpPhoneNumber = await _db.BPRPPhoneNumbers.FindAsync(phid);

            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);

            if (existingbprpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {phid} does not exist.", 404);

            if (!bpresponsiblePerson.PhoneNumbers.Any(x => x.Id == phid))
                throw new ApiProblemDetailsException($"BPResponsiblePerson with id: {bprpid} doesn't has this phone number", 409);

            _db.BPRPPhoneNumbers.Remove(existingbprpPhoneNumber);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion

        #region BPResponsible Person Email Operations

        [HttpGet]
        [Authorize(Permission.BPRPEmails.View)]
        [Route("/api/bpresponsiblepeople/{bprpid}/emails")]
        public async Task<ApiResponse> GetWithEmails(int bprpid, [FromQuery] QueryParams qp)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.FindAsync(bprpid);
            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);
            bpresponsiblePerson.Emails = bpresponsiblePerson.Emails.AsQueryable().Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToList();
            return new ApiResponse(_mapper.Map<BPResponsiblePersonWithEmailDto>(bpresponsiblePerson));
        }

        [HttpPost]
        [Authorize(Permission.BPRPEmails.Create)]
        [Route("/api/bpresponsiblepeople/{bprpid}/emails")]
        public async Task<ApiResponse> CreateEmail(int bprpid, BPRPEmailDto email)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.Where(x => x.Id == bprpid).FirstOrDefaultAsync();
            BPRPEmail bprpEmail = _mapper.Map<BPRPEmail>(email);

            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);

            ValidationResult result = _emailValidator.Validate(bprpEmail);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            _db.BPRPEmails.Add(bprpEmail);
            await _db.SaveChangesAsync();

            return new ApiResponse("Phone number added to responsible person succesfully.", _mapper.Map<BPRPEmailDto>(bprpEmail), 201);
        }

        [HttpPut]
        [Authorize(Permission.BPRPEmails.Edit)]
        [Route("/api/bpresponsiblepeople/{bprpid}/emails/{eid}")]
        public async Task<ApiResponse> UpdateEmail(int bprpid, int eid, BPRPEmailDto email)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.Where(x => x.Id == bprpid).FirstOrDefaultAsync();
            BPRPEmail existingEmail = await _db.BPRPEmails.FindAsync(eid);

            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);

            if (existingEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {eid} does not exist.", 404);

            if (!bpresponsiblePerson.Emails.Any(x => x.Id == eid))
                throw new ApiProblemDetailsException($"Responsinle with id: {bprpid} doesn't has this email", 409);

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
        [Authorize(Permission.BPRPEmails.Delete)]
        [Route("/api/bpresponsiblepeople/{bprpid}/emails/{eid}")]
        public async Task<ApiResponse> DeleteEmail(int bprpid, int eid)
        {
            BPResponsiblePerson bpresponsiblePerson = await _db.BPResponsiblePeople.Where(x => x.Id == bprpid).FirstOrDefaultAsync();
            BPRPEmail existingEmail = await _db.BPRPEmails.FindAsync(eid);

            if (bpresponsiblePerson == null)
                throw new ApiProblemDetailsException($"Record with id: {bprpid} does not exist.", 404);

            if (existingEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {eid} does not exist.", 404);

            if (!bpresponsiblePerson.Emails.Any(x => x.Id == eid))
                throw new ApiProblemDetailsException($"BPResponsiblePerson with id: {bprpid} doesn't has this email", 409);

            _db.BPRPEmails.Remove(existingEmail);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
