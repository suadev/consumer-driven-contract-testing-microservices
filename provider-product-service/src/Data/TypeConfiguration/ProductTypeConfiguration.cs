

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Data
{
    public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product(1, "Keyboard", 40),
                new Product(2, "Mouse", 20),
                new Product(3, "Monitor", 50),
                new Product(4, "Printer", 50),
                new Product(5, "Earphone", 20));
        }
    }
}
