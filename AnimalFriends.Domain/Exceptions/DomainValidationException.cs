using System.Net;

namespace AnimalFriends.Domain.Exceptions;

public class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message, HttpStatusCode.InternalServerError)
    {
    }
}