using AnimalFriends.Domain.Common;

namespace AnimalFriends.Sql;

public class SqlUnitOfWork : IUnitOfWork
{
    private readonly AnimalFriendsDbContext _context;

    public SqlUnitOfWork(AnimalFriendsDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        //this is largely pointless as implemented here as it is simply calling SaveChangesAsync.
        //In a fully-fledged system I would be using this method call to dispatch any messages/events
        //raised by changes to the domain during the lifetime of the DbContext.
        await _context.SaveChangesAsync(cancellationToken);
    }
}