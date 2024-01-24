using PryanikyTest.Domain.Enteties;

namespace PryanikyTest.Domain;

public class Customer : AppUser
{
    public ICollection<Order> Orders { get; set; }
}
