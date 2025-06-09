using Accidenta.Application.Contacts.DTO;
using Accidenta.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Contacts.Queries
{
    public record GetAllContactsQuery() : IRequest<IEnumerable<ContactDto>>;
    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllContactsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken ct)
        {
            var contacts = await _unitOfWork.Contacts.GetAllAsync(ct);
            return contacts.Select(c => new ContactDto(c));
        }
    }
}
