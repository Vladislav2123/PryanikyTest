using System.Collections.ObjectModel;

namespace PryanikyTest.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }

    public Guid SellerId {get; set;}
    public Seller Seller { get; set; }

    public Collection<ProductOrder> ProductOrders { get; set; }
}