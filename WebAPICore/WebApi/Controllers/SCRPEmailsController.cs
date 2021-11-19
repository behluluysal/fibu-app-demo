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
    public class SCRPEmailsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly SCRPEmailValidator _scrpEmailValidator;
        private readonly IMapper _mapper;
        public SCRPEmailsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _scrpEmailValidator = new SCRPEmailValidator(_db);
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.SCRPEmails.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.SCRPEmails.Count().ToString(),
                scrpemails = _mapper.Map<List<SCRPEmailDto>>(await _db.SCRPEmails.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.SCRPEmails.Create)]
        public async Task<ApiResponse> Create([FromBody] SCRPEmailDto scrpEmail)
        {
            SCRPEmail _sCRPEmail = _mapper.Map<SCRPEmail>(scrpEmail);

            ValidationResult result = _scrpEmailValidator.Validate(_sCRPEmail);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.SCRPEmails.AddAsync(_sCRPEmail);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<SCRPEmailDto>(_sCRPEmail), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.SCRPEmails.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            SCRPEmail scrpEmail = await _db.SCRPEmails.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (scrpEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<SCRPEmailDto>(scrpEmail));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.SCRPEmails.Edit)]
        public async Task<ApiResponse> Put(int id, SCRPEmailDto scrpEmail)
        {
            SCRPEmail existingSCRPEmail = await _db.SCRPEmails.FindAsync(id);
            if (existingSCRPEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingSCRPEmail.Email = scrpEmail.Email;

            ValidationResult result = _scrpEmailValidator.Validate(existingSCRPEmail);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            _db.Update(existingSCRPEmail);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.SCRPEmails.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var scrpEmail = await _db.SCRPEmails.FindAsync(id);
            if (scrpEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.SCRPEmails.Remove(scrpEmail);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
