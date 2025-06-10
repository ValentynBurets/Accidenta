using Accidenta.Application.Common.Mediator;
using Accidenta.Application.Contacts.DTO;
using Accidenta.Application.Exceptions;
using Accidenta.Domain.Interfaces;

namespace Accidenta.Application.Contacts.Queries;

public record GetContactByEmailQuery(string email);

public class GetContactByEmailQueryHandler : IQueryHandler<GetContactByEmailQuery, ContactDto>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetContactByEmailQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<ContactDto> Handle(GetContactByEmailQuery request, CancellationToken cancellationToken)
    {
        var contact = await _unitOfWork.Contacts.GetByEmailAsync(request.email, cancellationToken);
        if (contact is null)
            throw new NotFoundException("Contact not found.");

        return new ContactDto(contact);
    }
}
