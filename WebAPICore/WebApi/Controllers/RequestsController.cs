using AutoMapper;
using AutoWrapper.Wrappers;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Models.Chats;
using Core.Pagination;
using Core.Utility;
using DataStore.EF.Data;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorHtmlEmails.RazorClassLib.Models;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ProductRequestEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Fluent_Validation;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [JwtTokenValidateAttribute]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly RequestValidator _requestValidator;
        private readonly IMapper _mapper;
        private readonly IEmailSenderUseCases _emailSenderUseCases;

        public RequestsController(AppDbContext db, IMapper mapper, IEmailSenderUseCases emailSenderUseCases)
        {
            _db = db;
            _mapper = mapper;
            _emailSenderUseCases = emailSenderUseCases;
            _requestValidator = new RequestValidator(_db);
        }

        #region CRUD Operations

        [HttpGet]
        [Authorize(Permission.Requests.View)]
        public async Task<ApiResponse> GetRequests([FromQuery]QueryParams qp)
        {
            object response = new
            {
                count = _db.Requests.Count().ToString(),
                requests = _mapper.Map<List<RequestDto>>(await _db.Requests.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.Requests.Create)]
        public async Task<ApiResponse> PostRequests([FromBody] RequestDto request)
        {
            Request existingRequest = _mapper.Map<Request>(request);
            existingRequest.No = Guid.NewGuid().ToString().Substring(0, 5);
            existingRequest.Token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
            ValidationResult result = _requestValidator.Validate(existingRequest);
            existingRequest.BusinessPartner = null;

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            _db.Requests.Add(existingRequest);
            await _db.SaveChangesAsync();

            return new ApiResponse("New record has been created in the database.", _mapper.Map<RequestDto>(existingRequest), 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.Requests.View)]
        public async Task<ApiResponse> GetRequests(int id)
        {
            var request = await _db.Requests.FindAsync(id);

            if (request == null)
            {
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            }

            return new ApiResponse(_mapper.Map<RequestDto>(request));
        }

        [HttpPut("{id}")]
        [Authorize(Permission.Requests.Edit)]
        public async Task<ApiResponse> PutRequests(int id, [FromBody] RequestDto request)
        {
            if (id != request.Id)
            {
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            }

            Request existingRequest = await _db.Requests.FindAsync(id);
            Request.StatusValues oldStatus = existingRequest.Status;
            existingRequest.Status = request.Status;
            ValidationResult result = _requestValidator.Validate(existingRequest);

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
                _db.Update(existingRequest);
                await _db.SaveChangesAsync();
                if (request.Status == Core.Models.Request.StatusValues.Approved &&
                    oldStatus == Core.Models.Request.StatusValues.Created)
                {
                    foreach (var item in existingRequest.RequestedProducts)
                    {
                        item.Status = RequestedProduct.StatusValues.Approved;
                        _db.RequestedProducts.Update(item);
                    }
                    await _db.SaveChangesAsync();
                    await SendEmails(existingRequest.Id);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestsExists(id))
                {
                    throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.Requests.Delete)]
        public async Task<ApiResponse> DeleteRequests(int id)
        {
            var request = await _db.Requests.FindAsync(id);
            if (request == null)
            {
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            }

            _db.Requests.Remove(request);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        #endregion

        private bool RequestsExists(int id)
        {
            return _db.Requests.Any(e => e.Id == id);
        }

        #region External Methods

        private async Task SendEmails(int requestID)
        {
            try
            {
                Request existingRequest = await _db.Requests.FindAsync(requestID);

                // Get request tags
                List<int> relatedTags = new List<int>();
                foreach (var item in existingRequest.RequestedProducts)
                {
                    relatedTags.AddRange(item.Product.ProductTags.Select(x => x.Tag.Id).ToList());
                }
                relatedTags = relatedTags.Distinct().ToList();

                // Get related companies
                List<SupplierCompany> companies = _db.SupplierCompanies.Where(x => x.SupplierCompanyTags.Any(y => relatedTags.Contains(y.TagId))).ToList();
                companies = companies.Distinct().ToList();

                
                List<RequestedProduct> productsInRequest;
                List<int> intersectTags;
                foreach (var company in companies)
                {
                    intersectTags = relatedTags.Intersect(company.SupplierCompanyTags.Select(x => x.TagId)).ToList();
                    productsInRequest = existingRequest.RequestedProducts.Where(x => intersectTags.Intersect(x.Product.ProductTags.Select(x => x.TagId).ToList()).ToList().Count > 0).ToList();
                    
                    // Create Chats
                    var _chat = new Chat
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.Now,
                        SupplierCompanyId = company.Id,
                        RequestId = requestID,
                        IsNewMessage = false
                    };

                    _db.Chats.Add(_chat);

                    // Send Emails
                    await _emailSenderUseCases.CreateEmailSpecializedAsync(
                        company.Email,
                        "Product Requests",
                        new ProductRequestViewModel($"{Request.Host}/company-chat/{_chat.Id}",productsInRequest)
                        );
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }
}
