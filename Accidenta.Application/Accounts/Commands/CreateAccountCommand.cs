using Accidenta.Application.Common.Mediator;
using Accidenta.Application.DTO;
using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using FluentValidation;
using Serilog;
using ValidationException = FluentValidation.ValidationException;

namespace Accidenta.Application.Accounts.Commands;

public record CreateAccountCommand(CreateAccountRequest request);

public class CreateAccountHandler : ICommandHandler<CreateAccountCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly IValidator<CreateAccountRequest> _validator;

    public CreateAccountHandler(IUnitOfWork unitOfWork, ILogger logger, IValidator<CreateAccountRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var req = command.request;

        var result = _validator.Validate(req);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var existingAccount = await _unitOfWork.Accounts.GetByNameAsync(req.AccountName, cancellationToken);
        if (existingAccount != null)
        {
            throw new InvalidOperationException("Account with this name already exists");
        }

        var contact = await _unitOfWork.Contacts.GetByEmailAsync(req.Contact.Email, cancellationToken);

        if (contact == null)
        {
            contact = new Contact
            {
                FirstName = req.Contact.FirstName,
                LastName = req.Contact.LastName,
                Email = req.Contact.Email
            };

            await _unitOfWork.Contacts.AddAsync(contact, cancellationToken);
        }

        var account = new Account
        {
            Name = req.AccountName,
            Contact = contact
        };

        await _unitOfWork.Accounts.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Account created with ID {AccountId}", account.Id);
        return account.Id;
    }
}
