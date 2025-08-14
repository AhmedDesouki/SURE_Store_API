using Microsoft.EntityFrameworkCore;
using SURE_Store_API.Models;

namespace SURE_Store_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // باقي الـ DbSet للجداول الأخرى...
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        // جداول الكارت
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cart ↔ User (One-to-One)
            modelBuilder.Entity<Cart>()
                        .HasIndex(c => c.UserId)
                        .IsUnique();

            modelBuilder.Entity<Cart>()
                        .HasOne(c => c.User)
                        .WithOne() // أو .WithOne(u => u.Cart) لو User فيه Cart
                        .HasForeignKey<Cart>(c => c.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Cart ↔ CartItem (One-to-Many)
            modelBuilder.Entity<CartItem>()
                        .HasOne(ci => ci.Cart)
                        .WithMany(c => c.CartItems)
                        .HasForeignKey(ci => ci.CartId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Product ↔ CartItem (Many-to-One)
            modelBuilder.Entity<CartItem>()
                        .HasOne(ci => ci.Product)
                        .WithMany() // أو .WithMany(p => p.CartItems) لو فيه Navigation
                        .HasForeignKey(ci => ci.ProductId)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
