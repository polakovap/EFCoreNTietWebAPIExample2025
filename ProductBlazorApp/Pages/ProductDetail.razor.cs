using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using DataServices;
using Microsoft.AspNetCore.Components;
using ProductModel;

namespace ProductBlazorApp.Pages
{
    public partial class ProductDetail
    {
        //IEnumerable<Product> products;
        [Parameter]
        public int ID { get; set; }
        public Product Product { get; set; }

        [Inject]
        //IProductService? productService { get; set; }
        IHttpClientService? httpService { get; set; }
        [Inject]
        public IToastService toastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (httpService != null)
            {
                toastService.ShowInfo($"Products Loaded", settings => settings.IconType = IconType.None);
                if (await httpService.GetTokenAsync() != null)
                    Product = await httpService.getSingle<Product>("api/products/GetProduct/",ID);
            }

            if (Product == null)
            {
                toastService.ShowInfo($"No Product. Check Login", settings => settings.IconType = IconType.None);
            }
            await base.OnInitializedAsync();
        }

    }
}
