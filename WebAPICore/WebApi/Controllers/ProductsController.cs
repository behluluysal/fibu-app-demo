using AutoMapper;
using AutoWrapper.Wrappers;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using Core.Utility;
using DataStore.EF.Data;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ProductValidator _productValidator;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(AppDbContext db, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _productValidator = new ProductValidator(_db);
        }

        #region CRUD Operaitons

        [HttpGet]
        [Authorize(Permission.Products.View)]
        public async Task<ApiResponse> Get([FromQuery] QueryParams qp)
        {
            object response = new
            {
                count = _db.Products.Count().ToString(),
                products = _mapper.Map<List<ProductDto>>(await _db.Products.Where(qp.Filter).OrderBy(qp.Sort).Skip((qp.Page)).Take(qp.PostsPerPage).ToListAsync())
            };
            return new ApiResponse(response);
        }

        [HttpPost]
        [Authorize(Permission.Products.Create)]
        public async Task<ApiResponse> Create([FromBody] ProductDto product)
        {
            Product newProduct = _mapper.Map<Product>(product);
            
            if(product.Image != null)
            {
                IFormFile file = Base64ToImage(product);
                if (file == null)
                    throw new ApiProblemDetailsException($"Unprocessable image format", 400);
                string uniqueFileName = UploadedFile(file);
                newProduct.Image = uniqueFileName;
            }

            ValidationResult result = _productValidator.Validate(newProduct);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }

            await _db.Products.AddAsync(newProduct);
            await _db.SaveChangesAsync();
            return new ApiResponse("New record has been created in the database.", _mapper.Map<ProductDto>(newProduct) , 201);
        }

        [HttpGet("{id}")]
        [Authorize(Permission.Products.View)]
        public async Task<ApiResponse> GetById([FromRoute] int id)
        {
            Product product = await _db.Products.Where(x=>x.Id == id).FirstOrDefaultAsync();
            if (product == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);
            return new ApiResponse(_mapper.Map<ProductDto>(product));
        }


        [HttpPut("{id}")]
        [Authorize(Permission.Products.Edit)]
        public async Task<ApiResponse> Put(int id, ProductDto product)
        {
            Product existingProduct = await _db.Products.FindAsync(id);
            if (existingProduct == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);


            if (existingProduct.Image != null)
            {
                DeleteFile(existingProduct.Image);
                existingProduct.Image = null;
            }
                

            //mustang In this part, with every update, method deleted the old image and uploads the new oe even if the photo is not changed.
            if (product.Image != null)
            {
                IFormFile file = Base64ToImage(product);
                if (file == null)
                    throw new ApiProblemDetailsException($"Unprocessable image format", 400);
                string uniqueFileName = UploadedFile(file);
                existingProduct.Image = uniqueFileName;
            }
           

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            

            ValidationResult result = _productValidator.Validate(existingProduct);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
                }
                throw new ApiProblemDetailsException(ModelState);
            }
            _db.Update(existingProduct);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        [HttpDelete("{id}")]
        [Authorize(Permission.Products.Delete)]
        public async Task<ApiResponse> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            DeleteFile(product.Image);
            if (product == null)
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", 404);

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return new ApiResponse(204);
        }

        #endregion

        #region Product Tag Operations

        [HttpGet]
        [Authorize(Permission.Products.View)]
        [Route("/api/products/{pid}/tags")]
        public async Task<ApiResponse> GetTags(int pid)
        {
            Product product = await _db.Products.FindAsync(pid);
            if(product == null)
                throw new ApiProblemDetailsException($"Record with id: {pid} does not exist.", 404);
            return new ApiResponse(_mapper.Map<ProductWithTagDto>(product));
        }

        [HttpPost]
        [Authorize(Permission.Products.EditTag)]
        [Route("/api/products/{pid}/tags/{tid}")]
        public async Task<ApiResponse> AssignTag(int pid, int tid)
        {
            Product product = await _db.Products.Where(x => x.Id == pid).FirstOrDefaultAsync();
            Tag tag = await _db.Tags.Where(x => x.Id == tid).FirstOrDefaultAsync();

            if(product == null)
                throw new ApiProblemDetailsException($"Record with id: {pid} does not exist.", 404);
            if(tag == null)
                throw new ApiProblemDetailsException($"Record with id: {tid} does not exist.", 404);

            if(product.ProductTags.Any(x=>x.Tag == tag))
                throw new ApiProblemDetailsException($"Product with id: {pid} already has this tag", 409);

            ProductTag productTag = new ProductTag {Product = product, ProductId = product.Id, Tag = tag, TagId=tag.Id };
            product.ProductTags.Add(productTag);
            tag.ProductTags.Add(productTag);
            await _db.SaveChangesAsync();

            return new ApiResponse("Tag assigned to product succesfully.", _mapper.Map<ProductWithTagDto>(product));
        }

        [HttpDelete]
        [Authorize(Permission.Products.EditTag)]
        [Route("/api/products/{pid}/tags/{tid}")]
        public async Task<ApiResponse> WithdrawTag(int pid, int tid)
        {
            Product product = await _db.Products.Where(x => x.Id == pid).FirstOrDefaultAsync();
            Tag tag = await _db.Tags.Where(x => x.Id == tid).FirstOrDefaultAsync();

            if (product == null)
                throw new ApiProblemDetailsException($"Record with id: {pid} does not exist.", 404);
            if (tag == null)
                throw new ApiProblemDetailsException($"Record with id: {tid} does not exist.", 404);

            if (!product.ProductTags.Any(x => x.Tag == tag))
                throw new ApiProblemDetailsException($"Product with id: {pid} doesn't has this tag", 409);

            ProductTag productTag = await _db.ProductTags.Where(x => x.Tag == tag && x.Product == product).FirstOrDefaultAsync();
            _db.ProductTags.Remove(productTag);
            await _db.SaveChangesAsync();

            return new ApiResponse(204);
        }

        #endregion


        #region Image Operations
        
        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteFile(string file)
        {
            try
            {
                if (file != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    string filePath = Path.Combine(uploadsFolder, file);
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        private FormFile Base64ToImage(ProductDto model)
        {
            try
            {
                string[] parsedRadzenImage = model.Image.Split(",");
                if (!parsedRadzenImage[0].Contains("jpg") && !parsedRadzenImage[0].Contains("jpeg") && !parsedRadzenImage[0].Contains("png"))
                    return null;

                byte[] bytes = Convert.FromBase64String(parsedRadzenImage[1]);
                MemoryStream stream = new MemoryStream(bytes);

                FormFile file = new FormFile(stream, 0, bytes.Length, model.Name, (model.Name+".jpg"));
                return file;
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        #endregion

    }
}
