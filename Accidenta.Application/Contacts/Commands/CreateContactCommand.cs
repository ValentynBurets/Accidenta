using Accidenta.Application.Common.Mediator;
using Accidenta.Application.DTO;
using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Serilog;

namespace Accidenta.Application.Contacts.Commands;

public record CreateContactCommand(CreateContactRequest request);

public class CreateContactHandler : ICommandHandler<CreateContactCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public CreateContactHandler(IUnitOfWork unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        var req = command.request;
        var existing = await _unitOfWork.Contacts.GetByEmailAsync(req.Email, cancellationToken);

        if (existing != null)
        {
            throw new InvalidOperationException("Contact with the given email already exists");
        }

        var contact = new Contact
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email
        };

        await _unitOfWork.Contacts.AddAsync(contact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information("Contact created with ID {ContactId}", contact.Id);

        return contact.Id;
    }
}
