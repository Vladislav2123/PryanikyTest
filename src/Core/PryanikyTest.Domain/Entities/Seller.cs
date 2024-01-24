using PryanikyTest.Domain.Entities;

namespace PryanikyTest.Domain;

public class Seller : AppUser
{
    public ICollection<Product> Products { get; set; }
}
