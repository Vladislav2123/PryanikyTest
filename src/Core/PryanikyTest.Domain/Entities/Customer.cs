using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Domain;

public class Customer : AppUser
{
    public ICollection<Order> Orders { get; set; }
}
