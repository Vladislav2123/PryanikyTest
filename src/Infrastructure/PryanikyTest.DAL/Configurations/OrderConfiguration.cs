using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PryanikyTest.Domain.Enteties;

namespace PryanikyTest.DAL;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .HasKey(order => order.Id);

        builder
            .Property(order => order.CreationDate)
            .IsRequired();

        builder
            .Property(order => order.Completed)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .HasOne(order => order.Customer)
            .WithMany(customer => customer.Orders)
            .HasForeignKey(order => order.CustomerId);
    }
}
