using ProductModel;

namespace ProductBlazorApp.Services
{
    public interface IProductService
    {
        Task<List<Product>> getProducts();
        void PostProduct(Product p);

        Task<bool> login(string username, string password);
    }
}
