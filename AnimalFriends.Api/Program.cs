using System.Reflection;
using AnimalFriends.Api.ExceptionHandling;
using AnimalFriends.Application.Commands;
using AnimalFriends.Domain.Common;
using AnimalFriends.Domain.Customers;
using AnimalFriends.Sql;
using AnimalFriends.Sql.Customers;
using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<DomainExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Animal Friends API",
        Description = "A demonstration of a simple async API",
        Contact = new OpenApiContact
        {
            Name = "Olly Dawe",
            Email = "barndawe@gmail.com"
        }
    });
    
    options.EnableAnnotations();
});

//add the DB
builder.Services.AddDbContext<AnimalFriendsDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//bind repository and UoW
builder.Services
    .AddScoped<IUnitOfWork, SqlUnitOfWork>()
    .AddScoped<ICustomerRepository, CustomerRepository>();

//Mediatr, AutoMapper, and Fluent validation
builder.Services.AddMediatR(typeof(Program), typeof(CreateCustomerCommand));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

//show fluent validations in Swagger
builder.Services.AddFluentValidationRulesToSwagger();

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