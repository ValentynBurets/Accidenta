using Accidenta.Application.Common.DTO;
using Accidenta.Application.Common.Mediator;
using Accidenta.Application.Incidents.DTO;
using Accidenta.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accidenta.Application.Incidents.Queries;

public record GetAllIncidentsQuery(int Page = 1, int PageSize = 20);

public class GetAllIncidentsQueryHandler : IQueryHandler<GetAllIncidentsQuery, PagedResult<IncidentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllIncidentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<IncidentDto>> Handle(GetAllIncidentsQuery request, CancellationToken ct)
    {
        var query = _unitOfWork.Incidents.AsQueryable();

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<IncidentDto>
        {
            Items = items.Select(i => new IncidentDto(i)),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
