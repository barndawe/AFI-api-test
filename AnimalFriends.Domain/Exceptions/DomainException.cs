using System.Net;

namespace AnimalFriends.Domain.Exceptions;

public class DomainException : Exception
{
    public HttpStatusCode StatusCode { get; init; }

    public DomainException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}