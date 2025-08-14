using SURE_Store_API.Models;
using Microsoft.EntityFrameworkCore;

namespace SURE_Store_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)  // Accept database configuration options
            : base(options)  // Pass options to base IdentityDbContext constructor
        {
        }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
    }
}
