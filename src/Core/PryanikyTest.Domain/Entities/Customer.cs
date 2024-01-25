namespace PryanikyTest.Domain.Entities;

public class Customer : AppUser
{
    public ICollection<Order> Orders { get; set; }
}
