using Accidenta.Application.DTO;
using Accidenta.Application.Exceptions;
using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Serilog;

namespace Accidenta.Application.Incidents.Commands;

public class CreateIncident : IRequest<Guid>
{
    public CreateIncidentRequest Request { get; }
    public CreateIncident(CreateIncidentRequest request) => Request = request;
}

public class CreateIncidentHandler : IRequestHandler<CreateIncident, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly IValidator<CreateIncidentRequest> _validator;

    public CreateIncidentHandler(IUnitOfWork unitOfWork, ILogger logger, IValidator<CreateIncidentRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateIncident command, CancellationToken ct)
    {
        var req = command.Request;

        _logger.Information("Handling CreateIncidentCommand for AccountName: {AccountName}, Email: {Email}", req.AccountName, req.Email);

        var account = await _unitOfWork.Accounts.GetByNameAsync(req.AccountName, ct);

        if (account == null)
        {
            var msg = $"Account with name '{req.AccountName}' was not found.";
            _logger.Warning(msg);
            throw new NotFoundException(msg);
        }

        var validationResult = await _validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.Warning("Validation failed: {Errors}", errorMessage);
            throw new ValidationException(errorMessage);
        }

        var contact = await GetOrCreateAndLinkContactAsync(account, req, ct);
        var incident = await CreateIncidentAsync(account, req.Description, ct);

        _logger.Information("Incident created successfully: {IncidentId}", incident.Id);
        return incident.Id;
    }

    private async Task<Contact> GetOrCreateAndLinkContactAsync(Account account, CreateIncidentRequest req, CancellationToken ct)
    {
        var contact = await _unitOfWork.Contacts.GetByEmailAsync(req.Email, ct);

        if (contact == null)
        {
            contact = new Contact
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email
            };

            await _unitOfWork.Contacts.AddAsync(contact, ct);
            _logger.Information("Created new contact: {Email}", contact.Email);
        }
        else
        {
            contact.FirstName = req.FirstName;
            contact.LastName = req.LastName;
            _logger.Information("Updated existing contact: {Email}", contact.Email);
        }

        LinkContactToAccountIfNeeded(account, contact);

        return contact;
    }

    private void LinkContactToAccountIfNeeded(Account account, Contact contact)
    {
        if (account.ContactId != contact.Id)
        {
            account.Contact = contact;
            _logger.Information("Linked contact {ContactId} to account {AccountName}", contact.Id, account.Name);
        }
    }

    private async Task<Incident> CreateIncidentAsync(Account account, string description, CancellationToken ct)
    {
        var incident = new Incident
        {
            Account = account,
            Description = description
        };

        await _unitOfWork.Incidents.AddAsync(incident, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return incident;
    }
}
