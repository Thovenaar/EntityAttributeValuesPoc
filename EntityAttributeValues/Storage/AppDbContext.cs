using EntityAttributeValues.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityAttributeValues.Storage
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
