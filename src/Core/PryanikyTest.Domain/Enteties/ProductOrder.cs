using PryanikyTest.Domain.Enteties;

namespace PryanikyTest.Domain;

public class ProductOrder
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public int Amount { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
