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
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

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
                new Product { Id = 1, Name = "iPhone 15", Description = "Latest Apple smartphone", Price = 999.99m, ImageUrl = "https://images.unsplash.com/photo-1697284959152-32ef13855932?q=80&w=880&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", StockQuantity = 50, CategoryId = 1 },
                new Product { Id = 2, Name = "Samsung Galaxy S23", Description = "Latest Samsung smartphone", Price = 899.99m, ImageUrl = "https://m.media-amazon.com/images/I/51ngAkKqflL._AC_UY327_FMwebp_QL65_.jpg", StockQuantity = 30, CategoryId = 1 },
                new Product { Id = 3, Name = "Sony WH-1000XM5", Description = "Noise-cancelling headphones", Price = 349.99m, ImageUrl = "https://m.media-amazon.com/images/I/51hrEIBMDzL._AC_SL1320_.jpg", StockQuantity = 100, CategoryId = 1 },
                new Product { Id = 4, Name = "Nike Air Max", Description = "Comfortable running shoes", Price = 129.99m, ImageUrl = "https://m.media-amazon.com/images/I/61gG+NEOhmL._AC_SX575_.jpg", StockQuantity = 200, CategoryId = 2 },
                new Product { Id = 5, Name = "Adidas Ultraboost", Description = "High-performance running shoes", Price = 159.99m, ImageUrl = "https://m.media-amazon.com/images/I/71Kks1RxWhL._AC_SY695_.jpg", StockQuantity = 150, CategoryId = 2 },
                new Product { Id = 6, Name = "Levi's Jeans", Description = "Classic denim jeans", Price = 59.99m, ImageUrl = "https://m.media-amazon.com/images/I/51vL9b0iG5L._AC_SX679_.jpg", StockQuantity = 300, CategoryId = 2 },
                new Product { Id = 7, Name = "The Great Gatsby", Description = "Classic novel by F. Scott Fitzgerald", Price = 10.99m, ImageUrl = "https://m.media-amazon.com/images/I/61uYYec8joL._SL1500_.jpg", StockQuantity = 500, CategoryId = 3 },
                new Product { Id = 8, Name = "1984 by George Orwell", Description = "Dystopian novel about totalitarianism", Price = 12.99m, ImageUrl = "https://m.media-amazon.com/images/I/71wANojhEKL._SL1500_.jpg", StockQuantity = 400, CategoryId = 3 },
                new Product { Id = 9, Name = "To Kill a Mockingbird", Description = "Classic novel by Harper Lee", Price = 14.99m, ImageUrl = "https://m.media-amazon.com/images/I/81aY1lxk+9L._SL1500_.jpg", StockQuantity = 350, CategoryId = 3 },
                new Product { Id = 10, Name = "The Catcher in the Rye", Description = "Novel by J.D. Salinger", Price = 11.99m, ImageUrl = "https://m.media-amazon.com/images/I/81TRBjfC5fL._SL1500_.jpg", StockQuantity = 450, CategoryId = 3 }
               
            );
        }
    }
}
