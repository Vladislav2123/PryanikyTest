namespace PryanikyTest.Domain.Entities;

public class Seller : AppUser
{
    public ICollection<Product> Products { get; set; }
}
