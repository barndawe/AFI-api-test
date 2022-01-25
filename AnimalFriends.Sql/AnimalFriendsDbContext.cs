using AnimalFriends.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace AnimalFriends.Sql;

public class AnimalFriendsDbContext : DbContext
{
    public AnimalFriendsDbContext(DbContextOptions<AnimalFriendsDbContext> options): base(options)
    {
        
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Customer>()
            .HasKey(c => c.Id);

        //specifying the precise field lengths for our current valid max field sizes
        builder.Entity<Customer>()
            .Property(c => c.FirstName)
            .HasMaxLength(50);
        
        builder.Entity<Customer>()
            .Property(c => c.Surname)
            .HasMaxLength(50);
        
        builder.Entity<Customer>()
            .Property(c => c.PolicyReferenceNumber)
            .HasMaxLength(9);
        
        //RFC 3696 restricts email upper limit to be 254 chars.
        builder.Entity<Customer>()
            .Property(c => c.Email)
            .HasMaxLength(254);
    }
}