using MediatR;

namespace AnimalFriends.Application.Commands;

public class CreateCustomerCommand : IRequest<int>
{
    public string FirstName { get; init; }

    public string Surname { get; init; }

    public string PolicyReferenceNumber { get; init; }

    public DateTime? DateOfBirth { get; init; }

    public string Email { get; init; }
}