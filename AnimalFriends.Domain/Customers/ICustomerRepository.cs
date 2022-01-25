namespace AnimalFriends.Domain.Customers;

public interface ICustomerRepository
{
    Task<int> AddCustomerAsync(Customer customer, CancellationToken cancellationToken);
}