using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProductBlazorApp;
using ProductBlazorApp.Services;

namespace ProductBlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            // staandard scoped http client service replaced with custom service below

            builder.Services.AddHttpClient<IProductService, ProductService>(client => 
                            client.BaseAddress = new Uri("https://localhost:7218/"));
            await builder.Build().RunAsync();
        }
    }
}
