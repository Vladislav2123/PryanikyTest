namespace PryanikyTest.Domain.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }      // In this case, password is stores as an unhashed string
}
