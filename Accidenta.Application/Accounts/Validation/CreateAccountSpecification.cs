using Accidenta.Application.DTO;
using FluentValidation;

namespace Accidenta.Application.Accounts.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.AccountName)
            .NotEmpty()
            .WithMessage("Account name must not be empty");

        RuleFor(x => x.Contact)
            .NotNull()
            .WithMessage("Contact information must be provided");

        When(x => x.Contact != null, () =>
        {
            RuleFor(x => x.Contact.Email)
                .NotEmpty()
                .WithMessage("Contact email must not be empty");

            RuleFor(x => x.Contact.FirstName)
                .NotEmpty()
                .WithMessage("Contact first name must not be empty");

            RuleFor(x => x.Contact.LastName)
                .NotEmpty()
                .WithMessage("Contact last name must not be empty");
        });
    }
}
