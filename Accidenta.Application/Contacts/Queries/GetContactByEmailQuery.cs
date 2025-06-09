using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Contacts.Queries
{
    public class GetContactByEmailQuery : IRequest<Contact?>
    {
        public string Email { get; }
        public GetContactByEmailQuery(string email) => Email = email;
    }

    public class GetContactByEmailHandler : IRequestHandler<GetContactByEmailQuery, Contact?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetContactByEmailHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public Task<Contact?> Handle(GetContactByEmailQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.Contacts.GetByEmailAsync(request.Email, cancellationToken);
        }
    }
}
