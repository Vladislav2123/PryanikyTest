namespace PryanikyTest.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public ICollection<ProductOrder> ProductOrders { get; set; }
    public DateTime CreationDate { get; set; }

    public bool Completed { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
}
