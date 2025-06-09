using Accidenta.Application.DTO;
using Accidenta.Application.Exceptions;
using Accidenta.Application.Incidents.Specification;
using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Incidents.Commands
{
    public class CreateIncident : IRequest<string>
    {
        public CreateIncidentRequest Request { get; }
        public CreateIncident(CreateIncidentRequest request) => Request = request;
    }

    public class CreateIncidentHandler : IRequestHandler<CreateIncident, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CreateIncidentHandler(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(CreateIncident command, CancellationToken ct)
        {
            var req = command.Request;

            _logger.Information("Handling CreateIncidentCommand for AccountName: {AccountName}, Email: {Email}", req.AccountName, req.Email);

            var account = await _unitOfWork.Accounts.GetByNameAsync(req.AccountName, ct);

            var accountSpec = new AccountExistsSpecification();
            if (!accountSpec.IsSatisfiedBy(account))
            {
                _logger.Warning(accountSpec.ErrorMessage);
                throw new NotFoundException(accountSpec.ErrorMessage);
            }

            var contact = await GetOrCreateAndLinkContactAsync(account!, req, ct);

            var incident = await CreateIncidentAsync(account!, req.Description, ct);

            _logger.Information("Incident created successfully: {IncidentName}", incident.IncidentName);
            return incident.IncidentName;
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
}
