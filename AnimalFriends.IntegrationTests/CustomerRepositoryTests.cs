using System;
using System.Threading;
using System.Threading.Tasks;
using AnimalFriends.Domain.Customers;
using AnimalFriends.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AnimalFriends.IntegrationTests;

public class CustomerRepositoryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _context;

    public CustomerRepositoryTests(WebApplicationFactory<Program> context)
    {
        _context = context;
    }

    //I'm aware that most of these tests are only checking the 'happy path'. Due to the validation
    //on the Customer domain object it's pretty hard to break inserting a new customer.
    //Obviously with a fully-featured repo there would be a lot more to test!
    
    [Fact]
    public async Task Can_add_new_customer_with_valid_data_no_email()
    {
        var scope = _context.Services.CreateScope();
        
        var repo = scope.ServiceProvider.GetService<ICustomerRepository>();

        var customer = new Customer(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            PolicyNumberGenerator.GenerateValidPolicyNumber(),
            new DateTime(1970, 1, 1),
            null);

        customer.Id.Should().Be(0);

        var id = await repo.AddCustomerAsync(customer, CancellationToken.None);

        id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task Can_add_new_customer_with_valid_data_no_dob()
    {
        var scope = _context.Services.CreateScope();
        
        var repo = scope.ServiceProvider.GetService<ICustomerRepository>();

        var customer = new Customer(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            PolicyNumberGenerator.GenerateValidPolicyNumber(),
            null,
            "valid@email.com");

        customer.Id.Should().Be(0);

        var id = await repo.AddCustomerAsync(customer, CancellationToken.None);

        id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task Can_add_new_customer_with_valid_dob_and_email()
    {
        var scope = _context.Services.CreateScope();
        
        var repo = scope.ServiceProvider.GetService<ICustomerRepository>();

        var customer = new Customer(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            PolicyNumberGenerator.GenerateValidPolicyNumber(),
            new DateTime(1970, 1, 1),
            "valid@email.com");

        customer.Id.Should().Be(0);

        var id = await repo.AddCustomerAsync(customer, CancellationToken.None);

        id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task Cannot_add_customer_with_valid_but_too_long_email_expect_DbUpdateException()
    {
        var scope = _context.Services.CreateScope();
        
        var repo = scope.ServiceProvider.GetService<ICustomerRepository>();

        var customer = new Customer(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            PolicyNumberGenerator.GenerateValidPolicyNumber(),
            new DateTime(1970, 1, 1),
            "supercalifragilisticexpialidociousohwowthatsalotshorterwordthanithoughtandireallyneedthiswholeemailaddesstobeover254charactersandimonlyat141nowsoihavetokeepgoingbutimrunningoutofthingstosayiguessijustneedtokeepramblingonuntiligettothatmassivenumber@email.com");

        customer.Id.Should().Be(0);

        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await repo.AddCustomerAsync(customer, CancellationToken.None);
        });
    }
}