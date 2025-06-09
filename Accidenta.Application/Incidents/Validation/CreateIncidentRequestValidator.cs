using Accidenta.Application.DTO;
using FluentValidation;

namespace Accidenta.Application.Incidents.Validation;

public class CreateIncidentRequestValidator : AbstractValidator<CreateIncidentRequest>
{
    public CreateIncidentRequestValidator()
    {
        RuleFor(x => x.AccountName)
            .NotEmpty().WithMessage("Account name must not be empty");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Incident description is required");
    }
}
