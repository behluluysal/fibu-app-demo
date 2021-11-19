using App.Repository.ApiClient;
using Core.AutoMapperDtos;
using Core.Models;
using Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IWebApiExecuter _webApiExecuter;
        public ProductRepository(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        #region CRUD Operation Calls

        public async Task<(int, IEnumerable<ProductDto>)> GetAsync(QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/products?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ApiResponseIndex parsedResult = JsonSerializer.Deserialize<ApiResponseIndex>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (int.Parse(parsedResult.count), parsedResult.products);
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/products/{id}");
            ProductDto productDto = JsonSerializer.Deserialize<ProductDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }

        public async Task<ProductDto> CreateAsync(ProductDto product)
        {
            var response = await _webApiExecuter.InvokePost<dynamic>("api/products", product);
            ProductDto productDto = JsonSerializer.Deserialize<ProductDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return productDto;
        }

        public async Task UpdateAsync(int id, ProductDto product)
        {
            await _webApiExecuter.InvokePut($"api/products/{id}", product);
        }

        public async Task DeleteAsync(int id)
        {
            await _webApiExecuter.InvokeDelete($"api/products/{id}");
        }

        #endregion

        #region Product Tag Methods

        public async Task<(int, ProductWithTagDto)> GetProductTagsById(int id, QueryParams qp)
        {
            var response = await _webApiExecuter.InvokeGet<dynamic>($"api/products/{id}/tags?Page={qp.Page}&PostsPerPage={qp.PostsPerPage}&Filter={qp.Filter}&Sort={qp.Sort}");
            ProductWithTagDto productWithTagDtos = JsonSerializer.Deserialize<ProductWithTagDto>(response.GetProperty("result").ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return (productWithTagDtos.Tags.Count(), productWithTagDtos);
        }
        public async Task AssignTagAsync(int ProductId, int TagId)
        {
            Tag temp = null;
            await _webApiExecuter.InvokePost($"api/products/{ProductId}/tags/{TagId}", temp);
        }
        public async Task WithdrawTagAsync(int ProductId, int TagId)
        {
            await _webApiExecuter.InvokeDelete($"api/products/{ProductId}/tags/{TagId}");
        }
        #endregion

        public class ApiResponseIndex
        {
            [JsonPropertyName("count")]
            public string count { get; set; }
            [JsonPropertyName("products")]
            public IEnumerable<ProductDto> products { get; set; }
        }

    }
}
