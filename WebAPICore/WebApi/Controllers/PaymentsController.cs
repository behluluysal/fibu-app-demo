using AutoMapper;
using AutoWrapper.Wrappers;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using DataStore.EF.Data;
using FluentValidation.Results;
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
    [ApiController]
    [ApiVersion("1.0")]
    [JwtTokenValidateAttribute]
    public class PaymentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly PaymentValidator _paymentValidator;
        private readonly IMapper _mapper;
        public PaymentsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _paymentValidator = new PaymentValidator(_db);
            _mapper = mapper;
        }

        #region CRUD Operaitons

        [HttpGet]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.Payments.Count().ToString(),
                offers = _mapper.Map<List<PaymentDto>>(await _db.Payments.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        public async Task<ApiResponse> Create([FromBody] PaymentDto payment)
        {
            Payment _paymentDto = _mapper.Map<Payment>(payment);

            ValidationResult result = _paymentValidator.Validate(_paymentDto);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            try
            {
                await _db.Payments.AddAsync(_paymentDto);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            return new ApiResponse("New record has been created in the database.", _mapper.Map<PaymentDto>(_paymentDto), 201);
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            Payment payment = await _db.Payments.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (payment == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<PaymentDto>(payment));
        }


        [HttpPut("{id}")]
        public async Task<ApiResponse> Put(int id, PaymentDto payment)
        {
            Payment existingBPRPEmail = await _db.Payments.FindAsync(id);
            if (existingBPRPEmail == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            //empty

            ValidationResult result = _paymentValidator.Validate(existingBPRPEmail);
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
        public async Task<ApiResponse> Delete(int id)
        {
            var payment = await _db.Payments.FindAsync(id);
            if (payment == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.Payments.Remove(payment);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion
    }
}
