using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.DAL.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .HasKey(product => product.Id);

        builder
            .Property(product => product.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .Property(product => product.Description)
            .HasMaxLength(300);

        builder
            .Property(product => product.CreationDate)
            .IsRequired();

        builder
            .HasOne(product => product.Seller)
            .WithMany(seller => seller.Products)
            .HasForeignKey(product => product.SellerId);
    }
}
