using System;
using AnimalFriends.Domain.Customers;
using AnimalFriends.Domain.Exceptions;
using FluentValidation;
using Xunit;

namespace AnimalFriends.Domain.UnitTests;

public class CustomerTests
{
    [Fact]
    public void Can_create_customer_with_valid_data_no_email()
    {
        var _ = new Customer(
            "Olly",
            "Dawe",
            "AB-123456",
            new DateTime(1970, 01, 01),
            null);
    }
    
    [Fact]
    public void Can_create_customer_with_valid_data_no_dob()
    {
        var _ = new Customer(
            "Olly",
            "Dawe",
            "AB-123456",
            null,
            "fake@email.com");
    }

    [Fact]
    public void Can_create_customer_with_full_valid_data()
    {
        var _ = new Customer(
            "Olly",
            "Dawe",
            "AB-123456",
            new DateTime(1970, 01, 01),
            "fake@email.com");
    }

    [Theory]
    [InlineData("Olly", null)]
    [InlineData(null, "Dawe")]
    [InlineData("", "Dawe")]
    [InlineData("Olly", "")]
    [InlineData("O", "Dawe")]
    [InlineData("Olly", "D")]
    public void Cannot_create_customer_without_full_valid_length_name(string firstName, string surname)
    {
        var sut = () => new Customer(firstName,
            surname,
            "AB-123456",
            new DateTime(1970, 01, 01),
            "fake@email.com");

        Assert.Throws<DomainValidationException>(sut);
    }

    [Theory]
    [InlineData("ab-123456")]
    [InlineData("Ab-123456")]
    [InlineData("aB-123456")]
    [InlineData("A-123456")]
    [InlineData("AB-12345")]
    [InlineData("AB-1234567")]
    [InlineData("ABC-123456")]
    [InlineData(null)]
    public void Cannot_create_customer_without_valid_policy_number(string policyNumber)
    {
        var sut = () => new Customer("Olly",
            "Dawe",
            policyNumber,
            new DateTime(1970, 01, 01),
            "fake@email.com");

        Assert.Throws<DomainValidationException>(sut);
    }

    [Fact]
    public void Cannot_create_customer_with_neither_dob_or_email()
    {
        var sut = () => new Customer("Olly",
            "Dawe",
            "AB-123456",
            null,
            null);

        Assert.Throws<DomainValidationException>(sut);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Cannot_create_customer_under_18(bool passValidEmail)
    {
        var sut = () => new Customer("Olly",
            "Dawe",
            "AB-123456",
            DateTime.Now.AddYears(-17),
            passValidEmail ? "fake@email.com" : null);

        Assert.Throws<DomainValidationException>(sut);
    }


    [Theory]
    [InlineData("123@12.com", true)]
    [InlineData("123@12.com", false)]
    [InlineData("1234@1.com", true)]
    [InlineData("1234@1.com", false)]
    [InlineData("1234@12.org", true)]
    [InlineData("1234@12.org", false)]
    public void Cannot_create_customer_with_invalid_email(string email, bool passValidDoB)
    {
        var sut = () => new Customer("Olly",
            "Dawe",
            "AB-123456",
            passValidDoB ? new DateTime(1970,1,1) : null,
            email);

        Assert.Throws<DomainValidationException>(sut);
    }
}