using Accidenta.Application.Contacts.DTO;
using Accidenta.Application.Exceptions;
using Accidenta.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Contacts.Queries
{
    public record GetContactByIdQuery(Guid Id) : IRequest<ContactDto>;

    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContactByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ContactDto> Handle(GetContactByIdQuery request, CancellationToken ct)
        {
            var contact = await _unitOfWork.Contacts.GetByIdAsync(request.Id, ct);
            if (contact is null)
                throw new NotFoundException("Contact not found.");

            return new ContactDto(contact);
        }
    }
}
