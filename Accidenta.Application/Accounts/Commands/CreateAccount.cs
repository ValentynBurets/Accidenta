using Accidenta.Application.DTO;
using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Accounts.Commands
{
    public class CreateAccount : IRequest<Guid>
    {
        public CreateAccountRequest Request { get; }
        public CreateAccount(CreateAccountRequest request) => Request = request;
    }

    public class CreateAccountHandler : IRequestHandler<CreateAccount, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly ISpecification<CreateAccountRequest> _specification;

        public CreateAccountHandler(IUnitOfWork unitOfWork, ILogger logger, ISpecification<CreateAccountRequest> specification)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _specification = specification;
        }

        public async Task<Guid> Handle(CreateAccount command, CancellationToken cancellationToken)
        {
            var req = command.Request;

            if (!_specification.IsSatisfiedBy(req))
            {
                throw new ArgumentException("Invalid account creation request: Contact and account name must be provided with valid data.");
            }

            var existingAccount = await _unitOfWork.Accounts.GetByNameAsync(req.AccountName, cancellationToken);
            if (existingAccount != null)
            {
                throw new InvalidOperationException("Account with this name already exists.");
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
}
