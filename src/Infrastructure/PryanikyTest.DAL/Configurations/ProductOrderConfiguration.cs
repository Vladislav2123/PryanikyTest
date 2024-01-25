using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PryanikyTest.Domain.Entities;

namespace PryanikyTest.DAL;

public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrder>
{
    public void Configure(EntityTypeBuilder<ProductOrder> builder)
    {
        builder
            .HasKey(productOrder => productOrder.Id);

        builder
            .HasOne(productOrder => productOrder.Product)
            .WithMany(product => product.ProductOrders)
            .HasForeignKey(productOrder => productOrder.ProductId);

        builder
            .Property(productOrder => productOrder.Amount)
            .IsRequired()
            .HasDefaultValue(1);

        builder
            .HasOne(productOrder => productOrder.Order)
            .WithMany(order => order.ProductOrders)
            .HasForeignKey(productOrder => productOrder.OrderId);
    }
}
