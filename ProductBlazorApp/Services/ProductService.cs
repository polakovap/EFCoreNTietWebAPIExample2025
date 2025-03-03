using ProductModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ProductBlazorApp.Services
{
    public class ProductService : IProductService
    {
        private HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> getProducts()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // New Blazor Get
            try
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                var response = await _httpClient.GetFromJsonAsync<List<Product>>("api/Products", options);
                return response;
            }

            catch (Exception e)
            {
                Console.WriteLine("Error {0}", e.Message);
                return new List<Product>();
            }
        }

        public Task<bool> login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void PostProduct(Product p)
        {
            throw new NotImplementedException();
        }
    }
}
