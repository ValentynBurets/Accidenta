using Accidenta.Application.Accounts.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Application.Exceptions;
using Accidenta.Domain.Interfaces;

namespace Accidenta.Application.Accounts.Queries;

public record GetAccountByNameQuery(string Name);

public class GetAccountByNameQueryHandler : IQueryHandler<GetAccountByNameQuery, AccountDto>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAccountByNameQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<AccountDto> Handle(GetAccountByNameQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByNameAsync(request.Name, cancellationToken);
        if (account is null)
            throw new NotFoundException("Account not found");

        return new AccountDto(account);
    }
}
