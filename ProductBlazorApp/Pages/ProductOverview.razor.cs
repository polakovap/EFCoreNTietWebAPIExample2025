
using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using DataServices;
using Microsoft.AspNetCore.Components;
using ProductBlazorApp.Services;
using ProductModel;
using System.ComponentModel.DataAnnotations;

namespace ProductBlazorApp.Pages
{
    public partial class ProductOverview
    {
        List<Product> products = new List<Product>();

        [Inject]
        //IProductService? productService { get; set; }
        IHttpClientService? httpService { get; set; }
        [Inject]
        public IToastService toastService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //InitialiseProducts();
            //if(productService != null)
            //    products = await productService.getProducts();
            if (httpService != null)
            {
                toastService.ShowInfo($"Products Loaded", settings => settings.IconType = IconType.None);
                products = await httpService.getCollection<Product>("api/products");

            }

            if (products == null)
            {
                toastService.ShowInfo($"No Products Loaded. Check Login", settings => settings.IconType = IconType.None);
            }
            await base.OnInitializedAsync();
        }
        private void InitialiseProducts()
        {
            //products = new List<Product>
            //{
            //    new Product{ ID = 1, Description = "Chai", StockOnHand = 12},
            //    new Product{ ID = 2, Description = "Syrup", StockOnHand = 10},
            //    new Product{ ID = 3, Description = "Cajun Seasoning", StockOnHand = 200},

            //};
        }
    }
}
