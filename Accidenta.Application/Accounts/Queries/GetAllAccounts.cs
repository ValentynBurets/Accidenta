using Accidenta.Application.Accounts.DTO;
using Accidenta.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Accounts.Queries
{
    public record GetAllAccountsQuery() : IRequest<IEnumerable<AccountDto>>;
    public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAccountsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken ct)
        {
            var accounts = await _unitOfWork.Accounts.GetAllAsync(ct);
            return accounts.Select(a => new AccountDto(a));
        }
    }
}
