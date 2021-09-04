using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("product");
            modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
        }
    }
}