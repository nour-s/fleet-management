namespace Domain.Exceptions;

/// <summary>
/// Represent a failure in an operation that breaks the domain rules.
/// It does not represent a failure in the application or infrastructure.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}