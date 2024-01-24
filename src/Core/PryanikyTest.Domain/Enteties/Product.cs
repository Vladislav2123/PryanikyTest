using System.Collections.ObjectModel;

namespace PryanikyTest.Domain.Enteties;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }

    public Collection<ProductOrder> ProductOrders { get; set; }
}