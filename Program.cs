
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SURE_Store_API.Data;
using SURE_Store_API.Interfaces;
using SURE_Store_API.Models;
using SURE_Store_API.Repositories;
using SURE_Store_API.Services;
using System.Text;

namespace SURE_Store_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            // Register the DbContext with dependency injection
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Register the repository and unit of work for dependency injection
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            // Register the service for business logic
            builder.Services.AddScoped< SURE_Store_API.Services.IProductService, ProductService>();
            builder.Services.AddScoped<SURE_Store_API.Services.ICategoryService, CategoryService>();




            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Configure password complexity requirements for user accounts
                options.Password.RequireDigit = true;              // Password must contain at least one number (0-9)
                options.Password.RequireLowercase = true;          // Password must contain at least one lowercase letter (a-z)
                options.Password.RequireUppercase = true;          // Password must contain at least one uppercase letter (A-Z)
                options.Password.RequireNonAlphanumeric = true;    // Password must contain at least one special character (!@#$%^&*)
                options.Password.RequiredLength = 6;               // Minimum password length requirement
                options.User.RequireUniqueEmail = true;            // Each user must have a unique email address
            })
.AddEntityFrameworkStores<ApplicationDbContext>()      // Use Entity Framework Core for storing user data
.AddDefaultTokenProviders();

            var jwtSettings = builder.Configuration.GetSection("Jwt");  // Extract JWT configuration section from appsettings.json
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);     // Convert the secret key string to byte array for signing

            // Configure authentication to use JWT Bearer tokens as the default authentication scheme
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // Use JWT for user authentication
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;     // Use JWT for authentication challenges
            })
            .AddJwtBearer(options =>
            {
                // Configure JWT token validation parameters for security
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                    // Verify that the token was issued by the expected issuer
                    ValidateAudience = true,                  // Verify that the token is intended for the expected audience
                    ValidateLifetime = true,                  // Verify that the token has not expired
                    ValidateIssuerSigningKey = true,          // Verify the token signature using the signing key
                    ValidIssuer = jwtSettings["Issuer"],      // The expected issuer name from configuration
                    ValidAudience = jwtSettings["Audience"],  // The expected audience name from configuration
                    IssuerSigningKey = new SymmetricSecurityKey(key),  // The secret key used to validate token signatures
                    ClockSkew = TimeSpan.Zero                 // No tolerance for clock time differences between servers
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //Ahmed
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
