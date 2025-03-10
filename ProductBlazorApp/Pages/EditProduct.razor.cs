using Blazored.Toast.Services;
using DataServices;
using Microsoft.AspNetCore.Components;
using ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductBlazorApp.Pages
{
    public partial class EditProduct
    {
        [Parameter]
        public int ID { get; set; }
        public Product Product { get; set; }
        [Inject]
        public IHttpClientService httpService { get; set; }

        public string message { get; set; } = "Messages here";
        [Inject]
        public IToastService toastService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                if (await httpService.GetTokenAsync() != null)
                {
                    Product = await httpService.getSingle<Product>(@"api\Products\GetProduct\", ID);
                }
                else throw (new Exception("No Token Login found"));
               
                if (Product == null)
                {
                    throw (new Exception("No Product Found for ID" + ID.ToString()));
                }
                else Console.WriteLine(Product.ID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                NavigationManager.NavigateTo($"/Error/{e.Message}");
            }
        }
        private async void HandleValidSubmit()
        {
           
            await httpService.Put(@"api\Products\AddProduct\Update", Product);
            toastService.ShowInfo($"Product Updated {Product.Description}");  
            NavigationManager.NavigateTo("/");
        }

        private void HandleInvalidSubmit()
        {

        }
    }
}
