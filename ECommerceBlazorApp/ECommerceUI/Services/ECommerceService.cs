using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using ECommerceUI.Models;

namespace ECommerceUI.Services
{
    public class ECommerceService
    {
        private readonly HttpClient _httpClient;
        private readonly CacheService _cacheService;

        public ECommerceService(HttpClient httpClient, CacheService cacheService)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
        }

        // Products
        public Task<List<Product>> GetProductsAsync()
        {
            return _cacheService.GetOrAddAsync("Products", async () =>
            {
                var products = await _httpClient.GetFromJsonAsync<List<Product>>("api/Products");
                return products;
            }, TimeSpan.FromMinutes(5));
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            string cacheKey = $"Product_{id}";
            return _cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var product = await _httpClient.GetFromJsonAsync<Product>($"api/Products/{id}");
                return product;
            }, TimeSpan.FromMinutes(10));
        }

        public async Task CreateProductAsync(Product product)
        {
            await _httpClient.PostAsJsonAsync("api/Products", product);
            _cacheService.Remove("Products"); // Invalidate cache
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            await _httpClient.PutAsJsonAsync($"api/Products/{id}", product);
            _cacheService.Remove("Products"); // Invalidate cache
            _cacheService.Remove($"Product_{id}");
        }

        public async Task DeleteProductAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/Products/{id}");
            _cacheService.Remove("Products"); // Invalidate cache
            _cacheService.Remove($"Product_{id}");
        }

        // Categories
        public Task<List<Category>> GetCategoriesAsync()
        {
            return _cacheService.GetOrAddAsync("Categories", async () =>
            {
                var categories = await _httpClient.GetFromJsonAsync<List<Category>>("api/Categories");
                return categories;
            }, TimeSpan.FromMinutes(10));
        }

        public Task<Category> GetCategoryByIdAsync(int id)
        {
            string cacheKey = $"Category_{id}";
            return _cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var category = await _httpClient.GetFromJsonAsync<Category>($"api/Categories/{id}");
                return category;
            }, TimeSpan.FromMinutes(15));
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _httpClient.PostAsJsonAsync("api/Categories", category);
            _cacheService.Remove("Categories"); // Invalidate cache
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            await _httpClient.PutAsJsonAsync($"api/Categories/{id}", category);
            _cacheService.Remove("Categories"); // Invalidate cache
            _cacheService.Remove($"Category_{id}");
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/Categories/{id}");
            _cacheService.Remove("Categories"); // Invalidate cache
            _cacheService.Remove($"Category_{id}");
        }
    }
}
