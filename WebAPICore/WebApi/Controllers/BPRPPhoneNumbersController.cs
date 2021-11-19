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
    public class BPRPPhoneNumbersController : ControllerBase
    {
        private readonly BPRPPhoneNumberValidator _phoneNumberValidator;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public BPRPPhoneNumbersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _phoneNumberValidator = new BPRPPhoneNumberValidator(_db);
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.BPRPPhones.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.BPRPPhoneNumbers.Count().ToString(),
                bprpemails = _mapper.Map<List<BPRPPhoneNumberDto>>(await _db.BPRPPhoneNumbers.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.BPRPPhones.Create)]
        public async Task<ApiResponse> Create([FromBody] BPRPPhoneNumberDto bprpPhoneNumber)
        {
            BPRPPhoneNumber _bPRPPhoneNumber = _mapper.Map<BPRPPhoneNumber>(bprpPhoneNumber);
            ValidationResult result = _phoneNumberValidator.Validate(_bPRPPhoneNumber);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.BPRPPhoneNumbers.AddAsync(_bPRPPhoneNumber);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<BPRPPhoneNumberDto>(_bPRPPhoneNumber), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.BPRPPhones.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            BPRPPhoneNumber bprpPhoneNumber = await _db.BPRPPhoneNumbers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (bprpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<BPRPPhoneNumberDto>(bprpPhoneNumber));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.BPRPPhones.Edit)]
        public async Task<ApiResponse> Put(int id, BPRPPhoneNumberDto bprpPhoneNumber)
        {
            BPRPPhoneNumber existingScrpPhoneNumber = await _db.BPRPPhoneNumbers.FindAsync(id);
            if (existingScrpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingScrpPhoneNumber.Gsm = bprpPhoneNumber.Gsm;

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
        [Authorize(Permission.BPRPPhones.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var bprpPhoneNumber = await _db.BPRPPhoneNumbers.FindAsync(id);
            if (bprpPhoneNumber == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.BPRPPhoneNumbers.Remove(bprpPhoneNumber);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
