using AnimalFriends.Domain.Common;
using AnimalFriends.Domain.Customers;

namespace AnimalFriends.Sql.Customers;

public class CustomerRepository : ICustomerRepository
{
    private readonly AnimalFriendsDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerRepository(
        AnimalFriendsDbContext context,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> AddCustomerAsync(Customer customer,CancellationToken cancellationToken)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _context.Entry(customer).ReloadAsync(cancellationToken);
        
        return customer.Id;
    }
}