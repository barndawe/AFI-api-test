namespace AnimalFriends.Domain.Common;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}