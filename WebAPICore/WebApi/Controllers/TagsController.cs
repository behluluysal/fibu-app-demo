using AutoWrapper.Wrappers;
using Core.Models;
using Core.Pagination;
using DataStore.EF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using WebAPI.Fluent_Validation;
using FluentValidation.Results;
using AutoMapper;
using Core.AutoMapperDtos;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Core.Utility;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<TagsController> _logger;
        private readonly TagValidator _tagValidator;

        public TagsController(AppDbContext db, IMapper mapper, ILogger<TagsController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _tagValidator = new TagValidator(_db);
        }


        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.Tags.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.Tags.Count().ToString(),
                tags = _mapper.Map<IList<TagDto>>(await _db.Tags.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        
        [HttpPost]
        [Authorize(Permission.Tags.Create)]
        public async Task<ApiResponse> Create([FromBody] Tag tag)
        {
                ValidationResult result = _tagValidator.Validate(tag);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                    }
                    throw new ApiProblemDetailsException(ModelState);
                }

                await _db.Tags.AddAsync(tag);
                await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<TagDto>(tag), 201);
      
                

        }

        [HttpGet("{id}")]
        [Authorize(Permission.Tags.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            var tag = await _db.Tags.FindAsync(id);
            if (tag == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<TagDto>(tag));
        }

        [HttpPut("{id}")]
        [Authorize(Permission.Tags.Edit)]
        public async Task<ApiResponse> Put(int id, Tag tag)
        {
            Tag existingTag = await _db.Tags.FindAsync(id);
            if (existingTag == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            existingTag.Name = tag.Name;
            
            ValidationResult result = _tagValidator.Validate(tag);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            _db.Update(existingTag);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.Tags.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var tag = await _db.Tags.FindAsync(id);
            if (tag == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
