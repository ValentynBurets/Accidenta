using Accidenta.Application.Accounts.DTO;
using Accidenta.Application.Common.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accidenta.Application.Accounts.Queries;

public record GetAllAccountsQuery(int Page = 1, int PageSize = 10);

public class GetAllAccountsQueryHandler : IQueryHandler<GetAllAccountsQuery, PagedResult<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAccountsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken ct)
    {
        var query = _unitOfWork.Accounts.AsQueryable();

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<AccountDto>
        {
            Items = items.Select(a => new AccountDto(a)),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}