using Blazored.Toast;
using BlazorToastNotifications.Services;
using DataServices;
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

            //builder.Services.AddHttpClient<IProductService, ProductService>(client => 
            //                client.BaseAddress = new Uri("https://localhost:7218/"));
            builder.Services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7218/");
            });
            builder.Services.AddBlazoredToast();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddSingleton<AppState>();
            //builder.Services.AddScoped<ToastService>();

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var _localservice = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
                var _appState = scope.ServiceProvider.GetRequiredService<AppState>();
                // Clear the Token if set
                await _localservice.RemoveItem("token");
                // set loged out
                _appState.LoggedIn = false;
            }
            await app.RunAsync();

        }
    }
}
