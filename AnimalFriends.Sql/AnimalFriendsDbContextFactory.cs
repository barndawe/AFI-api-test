using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AnimalFriends.Sql;

public class AnimalFriendsDbContextFactory : IDesignTimeDbContextFactory<AnimalFriendsDbContext>
{
    public AnimalFriendsDbContext CreateDbContext(string[] args)
    {
        var appSettings = "./appSettings.json";
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Directory.GetCurrentDirectory() + appSettings)
            .Build();
        
        var dbContextBuilder = new DbContextOptionsBuilder<AnimalFriendsDbContext>();

        var connectionString = configuration.GetConnectionString("DesignConnection");

        dbContextBuilder.UseSqlServer(connectionString);

        return new AnimalFriendsDbContext(dbContextBuilder.Options);
    }
}