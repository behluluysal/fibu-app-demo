using AutoMapper;
using AutoWrapper.Wrappers;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.Utility;
using DataStore.EF.Data;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class BPRPEmailsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly BPRPEmailValidator _bprpEmailValidator;
        private readonly IMapper _mapper;
        public BPRPEmailsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _bprpEmailValidator = new BPRPEmailValidator(_db);
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.BPRPEmails.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.BPRPEmails.Count().ToString(),
                bprpemails = _mapper.Map<List<BPRPEmailDto>>(await _db.BPRPEmails.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.BPRPEmails.Create)]
        public async Task<ApiResponse> Create([FromBody] BPRPEmailDto bprpEmail)
        {
            BPRPEmail _bPRPEmailDto = _mapper.Map<BPRPEmail>(bprpEmail);

            ValidationResult result = _bprpEmailValidator.Validate(_bPRPEmailDto);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.BPRPEmails.AddAsync(_bPRPEmailDto);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<BPRPEmailDto>(_bPRPEmailDto), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.BPRPEmails.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            BPRPEmail bprpEmail = await _db.BPRPEmails.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (bprpEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<BPRPEmailDto>(bprpEmail));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.BPRPEmails.Edit)]
        public async Task<ApiResponse> Put(int id, BPRPEmailDto bprpEmail)
        {
            BPRPEmail existingBPRPEmail = await _db.BPRPEmails.FindAsync(id);
            if (existingBPRPEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingBPRPEmail.Email = bprpEmail.Email;

            ValidationResult result = _bprpEmailValidator.Validate(existingBPRPEmail);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            _db.Update(existingBPRPEmail);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.BPRPEmails.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var bprpEmail = await _db.BPRPEmails.FindAsync(id);
            if (bprpEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.BPRPEmails.Remove(bprpEmail);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
