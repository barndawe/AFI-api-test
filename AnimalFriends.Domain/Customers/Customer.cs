using AnimalFriends.Domain.Common;
using AnimalFriends.Domain.Exceptions;
using FluentValidation;

namespace AnimalFriends.Domain.Customers;

public class Customer : IValidatable
{
    public int Id { get; private set; }

    public string FirstName { get; private set; }

    public string Surname { get; private set; }

    public string PolicyReferenceNumber { get; private set; }

    public DateTime? DateOfBirth { get; private set; }

    public string Email { get; private set; }

    public Customer(
        string firstName,
        string surname,
        string policyReferenceNumber,
        DateTime? dateOfBirth,
        string email)
    {
        FirstName = firstName;
        Surname = surname;
        PolicyReferenceNumber = policyReferenceNumber;
        DateOfBirth = dateOfBirth;
        Email = email;

        ThrowIfInvalid();
    }

    public void ThrowIfInvalid()
    {
        var validator = new CustomerValidator();

        if (!validator.Validate(this).IsValid)
        {
            throw new DomainValidationException($"{nameof(Customer)} is not valid");
        }
    }

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(c => c.Surname).NotEmpty().MinimumLength(3).MaximumLength(50);

            // ensure policy ref is in format 'XX-999999', i.e precisely two capital letters, a single dash, and 6 numbers
            RuleFor(c => c.PolicyReferenceNumber).NotEmpty().Matches(@"^[A-Z]{2}-(\d){6}$");
            
            //either email or DoB must be present
            RuleFor(c => c.DateOfBirth).NotNull().When(c => string.IsNullOrWhiteSpace(c.Email));
            RuleFor(c => c.Email).NotEmpty().When(c => c.DateOfBirth is null);
            
            //policy holder must be at least 18 if specified
            RuleFor(c => c.DateOfBirth)
                .Must(d => (d ?? DateTime.MaxValue).Date.AddYears(18) <= DateTime.Now.Date)
                .When(c => c.DateOfBirth != null);
            
            //email must be at least 4 alpha-numeric chars, then @, then at least 2 alphanumeric chars
            //then .co.uk or .com if specified
            RuleFor(c => c.Email)
                .Matches(@"^[a-zA-Z0-9]{4,}\@[a-zA-Z0-9]{2,}(\.co\.uk|\.com)$");
        }
    }
}