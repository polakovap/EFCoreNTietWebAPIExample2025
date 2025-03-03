
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductModel;
using ProductWebAPI2025.DataLayer;
using System.Text;

namespace ProductWebAPI2025
{
    public class Program
    {

        public static void Main(string[] args)
        {
            // For CORS on localhost
            string LocalAllowSpecificOrigins = "_localAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddDbContext<ProductDBContext>();
            
            var connectionString = builder.Configuration.GetConnectionString("IdentityModelConnection") 
                ?? throw new InvalidOperationException("Connection string 'IdentityModelConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            var buisnessConnectionString = builder.Configuration.GetConnectionString("BusinessModelConnection")
                   ?? throw new InvalidOperationException("Connection string 'BusinessModelConnection' not found.");
            builder.Services.AddDbContext<ProductDBContext>(options =>
            //New Target assembly directive for migrations
                options.UseSqlServer(buisnessConnectionString, b => b.MigrationsAssembly("ProductModel")));

            builder.Services.AddTransient<IProduct<Product>, ProductRepository>();

            // adding user identity class  
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                cfg => cfg.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //add Authentication to the Web App
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie().AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Tokens:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: LocalAllowSpecificOrigins,
                                  builder =>
                                  {
                                      // current Blazor localhost endpoints See launchSettings.json in Blazor app
                                      builder.WithOrigins("https://localhost:7104",
                                                          "http://localhost:30571",
                                                          "http://localhost:5152")
                                                            .AllowAnyHeader()
                                                            .AllowAnyMethod();
                                  });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Web API", Version = "v1" });
                c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization Header using Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    }, new String[] {}
                }
                });
            });

            var app = builder.Build();

            // Retrieve the user context from the services container
            using (var scope = app.Services.CreateScope())
            {
                var _ctx = scope.ServiceProvider.GetRequiredService<ProductDBContext>();
                // Retrieve the IWebHostEnvironment for the Content Root even though we are not using the file system here
                var hostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                _ctx.Database.Migrate(); // Apply any pending migrations for Azure and local deployment if not already done
                // Create a new instance of the DbSeeder class and call the Seed method for suppliers
                DbSeeder dbSeeder = new DbSeeder(_ctx, hostEnvironment);
                dbSeeder.SeedSuppliers(); // seed method is in the dbseeder class
            }

            // Scope the user context for the application
            using (var scope = app.Services.CreateScope())
            {
                var _ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                _ctx.Database.Migrate(); // Apply any pending migrations for Azure and local deployment if not already done
                // Create a new instance of the ApplicationDbSeeder class and call the Seed method for users and roles
                ApplicationDbSeeder dbSeeder = new ApplicationDbSeeder(_ctx, userManager, roleManager);
                dbSeeder.Seed().Wait(); // seed method is in the dbseeder class
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseRouting();
            
            app.UseCors(LocalAllowSpecificOrigins);

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
