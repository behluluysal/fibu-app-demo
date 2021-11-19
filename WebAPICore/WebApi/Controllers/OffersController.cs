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
    public class OffersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly OfferValidator _offerValidator;
        private readonly IMapper _mapper;
        public OffersController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _offerValidator = new OfferValidator(_db);
            _mapper = mapper;
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.Offers.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.Offers.Count().ToString(),
                offers = _mapper.Map<List<OfferWithSupplierCompanyDto>>(await _db.Offers.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.Offers.Create)]
        public async Task<ApiResponse> Create([FromBody] OfferCreateDto offer)
        {
            Offer _offerDto = _mapper.Map<Offer>(offer);
           

            ValidationResult result = _offerValidator.Validate(_offerDto);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            await _db.Offers.AddAsync(_offerDto);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<OfferWithSupplierCompanyDto>(_offerDto), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.Offers.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            Offer offer = await _db.Offers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (offer == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<OfferWithSupplierCompanyDto>(offer));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.Offers.Edit)]
        public async Task<ApiResponse> Put(int id, OfferWithSupplierCompanyDto offer)
        {
            Offer existingOffer = await _db.Offers.FindAsync(id);
            if (existingOffer == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            //empty
            existingOffer.isConfirmedOffer = offer.isConfirmedOffer;

            ValidationResult result = _offerValidator.Validate(existingOffer);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            _db.Update(existingOffer);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.Offers.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var offer = await _db.Offers.FindAsync(id);
            if (offer == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.Offers.Remove(offer);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
