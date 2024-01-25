namespace PryanikyTest.Domain.Exceptions;

public class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException(string email, string entity) 
        : base($"Email \"{email}\" already in use by another {entity}") {}
}
