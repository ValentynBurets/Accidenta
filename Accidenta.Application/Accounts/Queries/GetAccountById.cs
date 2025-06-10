using Accidenta.Application.Accounts.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Application.Exceptions;
using Accidenta.Domain.Interfaces;

namespace Accidenta.Application.Accounts.Queries;

public record GetAccountByIdQuery(Guid Id);

public class GetAccountByIdQueryHandler : IQueryHandler<GetAccountByIdQuery, AccountDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountDto> Handle(GetAccountByIdQuery query, CancellationToken ct)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(query.Id, ct);
        if (account is null)
            throw new NotFoundException("Account not found");

        return new AccountDto(account);
    }
}
