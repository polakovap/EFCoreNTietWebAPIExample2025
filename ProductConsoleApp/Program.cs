using DataServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductModel;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;

namespace ProductConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo Cirl = new CultureInfo("ie-IE");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ProductDBContext.inProduction = true;
            var optionsBuilder = new DbContextOptionsBuilder<ProductDBContext>();
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = ProductCoreDB-2025");
            // if you use the parameterless ProductDBContext constructor
            // you will have to uncomment the OnConfiguring code to allow this constructor to work as it will have to find
            // the database connection without db context configuration
            // used in the web app configuration options
            using (ProductDBContext db = new ProductDBContext(optionsBuilder.Options)) 
            {                                                    
                foreach (Product product in db.Products)
                    Console.WriteLine("{0} Costs {1:C} ", product.Description, product.UnitPrice);
            }

            IHttpClientService client = new HttpClientService(new HttpClient() { BaseAddress = new Uri("https://localhost:7218") });

            if (client.login("paul.powell@atu.ie", "Rad302$1").Result)
            {
                Console.WriteLine("Logged In");
                Console.WriteLine("Token is: {0}", client.UserToken);
            }
            var products = client.getCollection<Product>("api/Products").Result;
            Console.WriteLine("Product Count {0}", products.Count);
            Console.ReadKey();
        }
    }
}
