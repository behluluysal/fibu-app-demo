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
    public class RequestedProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly RequestedProductValidator _productValidator;
        private readonly IMapper _mapper;
        public RequestedProductsController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _productValidator = new RequestedProductValidator(_db);
        }

        #region Crud Operations

        [HttpGet]
        [Authorize(Permission.RequestedProducts.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.RequestedProducts.Count().ToString(),
                requestedproducts = _mapper.Map<List<RequestedProductDto>>(await _db.RequestedProducts.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.RequestedProducts.Create)]
        public async Task<ApiResponse> Create([FromBody] RequestedProductDto product)
        {
            RequestedProduct existingProduct = _mapper.Map<RequestedProduct>(product);
            Request rq = await _db.Requests.FindAsync(product.RequestId);
            Product pt = await _db.Products.FindAsync(product.ProductId);

            existingProduct.Request = rq;
            existingProduct.Product = pt;
            existingProduct.Status = RequestedProduct.StatusValues.Created;

            ValidationResult result = _productValidator.Validate(existingProduct);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }


            await _db.RequestedProducts.AddAsync(existingProduct);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<RequestedProductDto>(existingProduct), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.RequestedProducts.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            RequestedProduct product = await _db.RequestedProducts.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (product == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<RequestedProductDto>(product));
        }

        [HttpPut("{id}")]
        [Authorize(Permission.RequestedProducts.Edit)]
        public async Task<ApiResponse> Put(int id, RequestedProductDto product)
        {
            RequestedProduct existingProduct = await _db.RequestedProducts.FindAsync(id);
            RequestedProduct.StatusValues oldStatus = existingProduct.Status;
            if (existingProduct == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);


            Request rq = await _db.Requests.FindAsync(product.RequestId);
            Product pt = await _db.Products.FindAsync(product.ProductId);
            existingProduct.Request = rq;
            existingProduct.Product = pt;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Deadline = product.Deadline;
            existingProduct.Status = product.Status;
            //mustang should send mails

            ValidationResult result = _productValidator.Validate(existingProduct);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            //check if all requested products are completed, then make the request completed
            if (product.Status == RequestedProduct.StatusValues.Confirmed &&
                    oldStatus == RequestedProduct.StatusValues.Approved)
            {
                bool flag = true;
                foreach (var item in existingProduct.Request.RequestedProducts)
                {
                    if (item.Status == RequestedProduct.StatusValues.Approved)
                        flag = false;
                }
                if (flag)
                {
                    existingProduct.Request.Status = Core.Models.Request.StatusValues.Completed;
                    _db.Requests.Update(existingProduct.Request);
                }    
            }
            _db.Update(existingProduct);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.RequestedProducts.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var product = await _db.RequestedProducts.FindAsync(id);
            if (product == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.RequestedProducts.Remove(product);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion

    }
}