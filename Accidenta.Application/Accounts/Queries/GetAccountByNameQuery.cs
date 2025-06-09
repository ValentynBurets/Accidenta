using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using MediatR;

namespace Accidenta.Application.Accounts.Queries;

public class GetAccountByNameQuery : IRequest<Account?>
{
    public string Name { get; }
    public GetAccountByNameQuery(string name) => Name = name;
}

public class GetAccountByNameHandler : IRequestHandler<GetAccountByNameQuery, Account?>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAccountByNameHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public Task<Account?> Handle(GetAccountByNameQuery request, CancellationToken cancellationToken)
    {
        return _unitOfWork.Accounts.GetByNameAsync(request.Name, cancellationToken);
    }
}
