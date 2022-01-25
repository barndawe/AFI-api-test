using AnimalFriends.Application.Commands;
using AnimalFriends.Domain.Customers;
using MediatR;

namespace AnimalFriends.Application.Handlers;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, int>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer(
            request.FirstName,
            request.Surname,
            request.PolicyReferenceNumber,
            request.DateOfBirth,
            request.Email);

        return await _customerRepository.AddCustomerAsync(customer, cancellationToken);
    }
}