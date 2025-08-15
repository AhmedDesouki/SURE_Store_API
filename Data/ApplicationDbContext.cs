using SURE_Store_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SURE_Store_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> //why these last two parameters?!!!!!!!!!!!!!!!
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Pass options to base DbContext constructor
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Decimal precision settings
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CartItem>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Devices and gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "Apparel and accessories" },
                new Category { Id = 3, Name = "Books", Description = "Literature and educational materials" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone 15", Description = "Latest Apple smartphone", Price = 999.99m, ImageUrl = "https://example.com/iphone15.jpg", StockQuantity = 50, CategoryId = 1 },
                new Product { Id = 2, Name = "Samsung Galaxy S23", Description = "Latest Samsung smartphone", Price = 899.99m, ImageUrl = "https://example.com/galaxys23.jpg", StockQuantity = 30, CategoryId = 1 },
                new Product { Id = 3, Name = "Sony WH-1000XM5", Description = "Noise-cancelling headphones", Price = 349.99m, ImageUrl = "https://example.com/sonyheadphones.jpg", StockQuantity = 100, CategoryId = 1 },
                new Product { Id = 4, Name = "Nike Air Max", Description = "Comfortable running shoes", Price = 129.99m, ImageUrl = "https://example.com/nikeairmax.jpg", StockQuantity = 200, CategoryId = 2 },
                new Product { Id = 5, Name = "Adidas Ultraboost", Description = "High-performance running shoes", Price = 159.99m, ImageUrl = "https://example.com/adidasultraboost.jpg", StockQuantity = 150, CategoryId = 2 },
                new Product { Id = 6, Name = "Levi's Jeans", Description = "Classic denim jeans", Price = 59.99m, ImageUrl = "https://example.com/levisjeans.jpg", StockQuantity = 300, CategoryId = 2 },
                new Product { Id = 7, Name = "The Great Gatsby", Description = "Classic novel by F. Scott Fitzgerald", Price = 10.99m, ImageUrl = "https://example.com/greatgatsby.jpg", StockQuantity = 500, CategoryId = 3 },
                new Product { Id = 8, Name = "1984 by George Orwell", Description = "Dystopian novel about totalitarianism", Price = 12.99m, ImageUrl = "https://example.com/1984.jpg", StockQuantity = 400, CategoryId = 3 },
                new Product { Id = 9, Name = "To Kill a Mockingbird", Description = "Classic novel by Harper Lee", Price = 14.99m, ImageUrl = "https://example.com/tokillamockingbird.jpg", StockQuantity = 350, CategoryId = 3 },
                new Product { Id = 10, Name = "The Catcher in the Rye", Description = "Novel by J.D. Salinger", Price = 11.99m, ImageUrl = "https://example.com/catcherintherye.jpg", StockQuantity = 450, CategoryId = 3 },
                new Product { Id = 11, Name = "MacBook Pro", Description = "Apple's high-performance laptop", Price = 1999.99m, ImageUrl = "https://example.com/macbookpro.jpg", StockQuantity = 20, CategoryId = 1 },
                new Product { Id = 12, Name = "Dell XPS 13", Description = "Compact and powerful laptop", Price = 1299.99m, ImageUrl = "https://example.com/dellxps13.jpg", StockQuantity = 25, CategoryId = 1 },
                new Product { Id = 13, Name = "Bose QuietComfort 35 II", Description = "Wireless noise-cancelling headphones", Price = 299.99m, ImageUrl = "https://example.com/boseqc35.jpg", StockQuantity = 80, CategoryId = 1 },
                new Product { Id = 14, Name = "Canon EOS R5", Description = "Professional mirrorless camera", Price = 3899.99m, ImageUrl = "https://example.com/canoneosr5.jpg", StockQuantity = 15, CategoryId = 1 },
                new Product { Id = 15, Name = "Sony A7 III", Description = "Full-frame mirrorless camera", Price = 1999.99m, ImageUrl = "https://example.com/sonya7iii.jpg", StockQuantity = 30, CategoryId = 1 },
                new Product { Id = 16, Name = "Fitbit Charge 5", Description = "Advanced fitness tracker with GPS", Price = 149.99m, ImageUrl = "https://example.com/fitbitcharge5.jpg", StockQuantity = 100, CategoryId = 1 },
                new Product { Id = 17, Name = "Samsung Galaxy Tab S8", Description = "Premium Android tablet with S Pen support", Price = 699.99m, ImageUrl = "https://example.com/galaxytabs8.jpg", StockQuantity = 40, CategoryId = 1 },
                new Product { Id = 18, Name = "Apple Watch Series 7", Description = "Smartwatch with health tracking features", Price = 399.99m, ImageUrl = "https://example.com/applewatchseries7.jpg", StockQuantity = 60, CategoryId = 1 }
            );
        }
    }
}
