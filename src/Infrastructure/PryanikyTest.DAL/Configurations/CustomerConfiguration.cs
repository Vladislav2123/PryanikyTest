using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PryanikyTest.Domain;

namespace PryanikyTest.DAL.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
            .HasKey(customer => customer.Id);

        builder
            .HasIndex(customer => customer.Email)
            .IsUnique();

        builder
            .Property(customer => customer.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .Property(customer => customer.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(customer => customer.Password)
            .IsRequired()
            .HasMaxLength(32);
    }
}
