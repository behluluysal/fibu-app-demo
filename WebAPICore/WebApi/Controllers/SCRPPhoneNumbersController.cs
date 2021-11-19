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
    public class SCRPPhoneNumbersController : ControllerBase
    {
        private readonly SCRPPhoneNumberValidator _phoneNumberValidator;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public SCRPPhoneNumbersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _phoneNumberValidator = new SCRPPhoneNumberValidator(_db);
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.SCRPPhones.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.SCRPPhoneNumbers.Count().ToString(),
                scrpphonenumbers = _mapper.Map<List<SCRPPhoneNumberDto>>(await _db.SCRPPhoneNumbers.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.SCRPPhones.Create)]
        public async Task<ApiResponse> Create([FromBody] SCRPPhoneNumberDto scrpPhoneNumber)
        {
            SCRPPhoneNumber _sCRPPhoneNumber = _mapper.Map<SCRPPhoneNumber>(scrpPhoneNumber);

            ValidationResult result = _phoneNumberValidator.Validate(_sCRPPhoneNumber);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.SCRPPhoneNumbers.AddAsync(_sCRPPhoneNumber);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<SCRPPhoneNumberDto>(_sCRPPhoneNumber), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.SCRPPhones.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            SCRPPhoneNumber scrpPhoneNumber = await _db.SCRPPhoneNumbers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (scrpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<SCRPPhoneNumberDto>(scrpPhoneNumber));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.SCRPPhones.Edit)]
        public async Task<ApiResponse> Put(int id, SCRPPhoneNumberDto scrpPhoneNumber)
        {
            SCRPPhoneNumber existingScrpPhoneNumber = await _db.SCRPPhoneNumbers.FindAsync(id);
            if (existingScrpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingScrpPhoneNumber.Gsm = scrpPhoneNumber.Gsm;

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

        [HttpDelete("{id}")]
        [Authorize(Permission.SCRPPhones.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var scrpPhoneNumber = await _db.SCRPPhoneNumbers.FindAsync(id);
            if (scrpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.SCRPPhoneNumbers.Remove(scrpPhoneNumber);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
