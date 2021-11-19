using App.Repository;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UseCases
{
    public class ProductsScreenUseCases : IProductsScreenUseCases
    {
        private readonly IProductRepository _productRepository;
        private readonly ITagRepository _tagRepository;

        public ProductsScreenUseCases(IProductRepository productRepository, ITagRepository tagRepository)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
        }

        #region CRUD Methods

        public async Task<(int, IEnumerable<ProductDto>)> ViewProductsAsync(QueryParams qp)
        {
            return await _productRepository.GetAsync(qp);
        }

        public async Task<ProductDto> CreateProductAsync(ProductDto product)
        {

            return await _productRepository.CreateAsync(product);
        }

        public async Task<ProductDto> ViewProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task UpdateProductAsync(int id, ProductDto product)
        {
            await _productRepository.UpdateAsync(id, product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        #endregion

        #region Product Tag Methods

        public async Task<(int, ProductWithTagDto)> ViewProductTagsAsync(int pid, QueryParams qp)
        {
            return await _productRepository.GetProductTagsById(pid, qp);
        }
        public async Task AssignTagToProductAsync(int ProductId, int TagId)
        {
            await _productRepository.AssignTagAsync(ProductId, TagId);
        }

        public async Task WithdrawTagFromProductAsync(int ProductId, int TagId)
        {
            await _productRepository.WithdrawTagAsync(ProductId, TagId);
        }

        //mustang unused method
        public async Task<IEnumerable<TagDto>> ViewUnassignedTagsAsync(int pid, QueryParams qp)
        {
            try
            {
                var response = await _productRepository.GetProductTagsById(pid, qp);
                ProductWithTagDto productWithTagDtos = response.Item2;
                var response2 = await _tagRepository.GetAsync(new QueryParams());
                IEnumerable<TagDto> allTags = response2.Item2;
                //var allTags2 = allTags.Except(productWithTagDtos.Tags.ToList()).ToList(); mustang except is not working
                List<TagDto> returnTags = new List<TagDto>();

                foreach (var item in allTags)
                {
                    if(!productWithTagDtos.Tags.Any(x=>x.Id == item.Id))
                        returnTags.Add(item);
                }
                return returnTags;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
