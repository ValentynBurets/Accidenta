using Accidenta.Application.Accounts.DTO;
using Accidenta.Application.Exceptions;
using Accidenta.Domain.Interfaces;
using MediatR;

namespace Accidenta.Application.Accounts.Queries;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken ct)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.Id, ct);
        if (account is null)
            throw new NotFoundException("Account not found");

        return new AccountDto(account);
    }
}
