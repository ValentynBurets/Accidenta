using Accidenta.Application.Common.DTO;
using Accidenta.Application.Contacts.DTO;
using Accidenta.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accidenta.Application.Contacts.Queries;

public record GetAllContactsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<ContactDto>>;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, PagedResult<ContactDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllContactsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken ct)
    {
        var query = _unitOfWork.Contacts.AsQueryable();

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<ContactDto>
        {
            Items = items.Select(c => new ContactDto(c)),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}

