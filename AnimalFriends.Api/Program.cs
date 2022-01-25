using AnimalFriends.Domain.Common;
using AnimalFriends.Domain.Customers;
using AnimalFriends.Sql;
using AnimalFriends.Sql.Customers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add the DB
builder.Services.AddDbContext<AnimalFriendsDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//bind repository and UoW
builder.Services
    .AddScoped<IUnitOfWork, SqlUnitOfWork>()
    .AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

EnsureDatabaseCreated(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void EnsureDatabaseCreated(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    
    try
    {
        var context = services.GetRequiredService<AnimalFriendsDbContext>();

        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred bouncing the DB.");
    }
}

//for integration testing purposes
public partial class Program { }