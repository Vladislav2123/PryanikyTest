using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.DAL.Configurations;

public class SellerConfiguration : IEntityTypeConfiguration<Seller>
{
    public void Configure(EntityTypeBuilder<Seller> builder)
    {
        builder
            .HasKey(seller => seller.Id);

        builder
            .HasIndex(seller => seller.Email)
            .IsUnique();

        builder
            .Property(seller => seller.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .Property(seller => seller.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(seller => seller.Password)
            .IsRequired()
            .HasMaxLength(32);
    }
}
