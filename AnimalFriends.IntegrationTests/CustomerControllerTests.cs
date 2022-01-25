using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnimalFriends.Api.RequestModels;
using AnimalFriends.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AnimalFriends.IntegrationTests;

public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _context;

    public CustomerControllerTests(WebApplicationFactory<Program> context)
    {
        _context = context;
    }

    [Fact]
    public async Task Can_create_new_customer_with_valid_data_no_email()
    {
        var client = _context.CreateClient();

        var response = await client.PostAsJsonAsync("/customers",
            new CreateCustomerRequest
            {
                FirstName = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                PolicyReferenceNumber = PolicyNumberGenerator.GenerateValidPolicyNumber(),
                DateOfBirth = new DateTime(1970, 1, 1)
            });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();

        int.TryParse(content, out var id).Should().BeTrue();
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Can_create_new_customer_with_valid_data_no_dob()
    {
        var client = _context.CreateClient();

        var response = await client.PostAsJsonAsync("/customers",
            new CreateCustomerRequest
            {
                FirstName = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                PolicyReferenceNumber = PolicyNumberGenerator.GenerateValidPolicyNumber(),
                Email = "valid@email.com"
            });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();

        int.TryParse(content, out var id).Should().BeTrue();
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Can_create_new_customer_with_valid_email_and_dob()
    {
        var client = _context.CreateClient();

        var response = await client.PostAsJsonAsync("/customers",
            new CreateCustomerRequest
            {
                FirstName = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                PolicyReferenceNumber = PolicyNumberGenerator.GenerateValidPolicyNumber(),
                DateOfBirth = new DateTime(1970, 1, 1),
                Email = "valid@email.com"
            });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();

        int.TryParse(content, out var id).Should().BeTrue();
        id.Should().BeGreaterThan(0);
    }
    
    [Theory]
    [InlineData("O", "Dawe", "AB-123466", true, "valid@email.com")]
    [InlineData("Olly", "D", "AB-123466", true, "valid@email.com")]
    [InlineData("Olly", "D", null, true, "valid@email.com")]
    [InlineData("Olly", "D", "A-12345", true, "valid@email.com")]
    [InlineData("Olly", "D", "AB-123466", false, null)]
    [InlineData("Olly", "D", "AB-123466", true, "invalid@notvalid.org")]
    [InlineData("Olly", "D", "AB-123466", false, "invalid@notvalid.org")]
    public async Task Cannot_create_customer_without_valid_data_expect_400(
        string firstName,
        string surname,
        string policyNumber,
        bool passValidDob,
        string emailAddress)
    {
        var client = _context.CreateClient();

        var response = await client.PostAsJsonAsync("/customers",
            new CreateCustomerRequest
            {
                FirstName = firstName,
                Surname = surname,
                PolicyReferenceNumber = policyNumber,
                DateOfBirth = passValidDob ? new DateTime(1970, 1, 1) : null,
                Email = emailAddress
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Cannot_create_customer_under_18_expect_400()
    {
        var client = _context.CreateClient();

        var response = await client.PostAsJsonAsync("/customers",
            new CreateCustomerRequest
            {
                FirstName = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                PolicyReferenceNumber = PolicyNumberGenerator.GenerateValidPolicyNumber(),
                DateOfBirth = DateTime.Now.AddYears(-17),
                Email = "valid@email.com"
            });
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Cannot_create_customer_with_valid_but_too_long_email_expect_400()
    {
        var client = _context.CreateClient();

        var response = await client.PostAsJsonAsync("/customers",
            new CreateCustomerRequest
            {
                FirstName = Guid.NewGuid().ToString(),
                Surname = Guid.NewGuid().ToString(),
                PolicyReferenceNumber = PolicyNumberGenerator.GenerateValidPolicyNumber(),
                DateOfBirth = new DateTime(1970,1,1),
                Email = "supercalifragilisticexpialidociousohwowthatsalotshorterwordthanithoughtandireallyneedthiswholeemailaddesstobeover254charactersandimonlyat141nowsoihavetokeepgoingbutimrunningoutofthingstosayiguessijustneedtokeepramblingonuntiligettothatmassivenumber@email.com"
            });
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}