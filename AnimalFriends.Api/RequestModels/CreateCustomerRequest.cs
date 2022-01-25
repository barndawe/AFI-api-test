using FluentValidation;
using Swashbuckle.AspNetCore.Annotations;

namespace AnimalFriends.Api.RequestModels;

public class CreateCustomerRequest
{
    [SwaggerSchema("The customer's first name")]
    public string FirstName { get; set; }

    [SwaggerSchema("The customer's surname")]
    public string Surname { get; set; }

    [SwaggerSchema("The customer's policy reference number")]
    public string PolicyReferenceNumber { get; set; }

    [SwaggerSchema("The customer's date of birth")]
    public DateTime? DateOfBirth { get; set; }

    [SwaggerSchema("The customer's email address")]
    public string Email { get; set; }
}

public class CustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    //Validating on both the API request and the domain object as IMHO:
    //- domain objects should never be allowed to be created or saved in an invalid state
    //- we can add nicer (for the end user) validation messages on the failed API call
    //- in future the domain objects mey be created from internal events as well, so need their own validation.
    
    //Due to the simple nature of the Customer domain model's validation requirements the validation is
    //identical on both the create request and the object itself. For more complex domain objects (i.e. with
    //states such as 'Active', 'Archived', 'Deleted', etc) the validation would be more correspondingly more complex. 
    public CustomerValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(c => c.Surname).NotEmpty().MinimumLength(3).MaximumLength(50);

        // ensure policy ref is in format 'XX-999999', i.e precisely two capital letters, a single dash, and 6 numbers
        RuleFor(c => c.PolicyReferenceNumber).NotEmpty().Matches(@"^[A-Z]{2}-(\d){6}$");
            
        //either email or DoB must be present
        RuleFor(c => c.DateOfBirth).NotNull().When(c => string.IsNullOrWhiteSpace(c.Email))
            .WithMessage("Must specify date of birth when email is not specified");
        RuleFor(c => c.Email).NotEmpty().When(c => c.DateOfBirth is null)
            .WithMessage("Must specify email when date of birth is not specified");
            
        //policy holder must be at least 18 if specified
        RuleFor(c => c.DateOfBirth)
            .Must(d => (d ?? DateTime.MaxValue).Date.AddYears(18) <= DateTime.Now.Date)
            .When(c => c.DateOfBirth != null).WithMessage("Customer must be at least 18 years old");
            
        //email must be at least 4 alpha-numeric chars, then @, then at least 2 alphanumeric chars
        //then .co.uk or .com if specified
        //no longer than 254 chars
        RuleFor(c => c.Email)
            .Matches(@"^[a-zA-Z0-9]{4,}\@[a-zA-Z0-9]{2,}(\.co\.uk|\.com)$").MaximumLength(254);
    }
}