using Accidenta.Application.Exceptions;
using Accidenta.Application.Incidents.DTO;
using Accidenta.Domain.Interfaces;
using MediatR;

namespace Accidenta.Application.Incidents.Queries;

public record GetIncidentByIdQuery(Guid Id) : IRequest<IncidentDto>;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, IncidentDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIncidentByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IncidentDto> Handle(GetIncidentByIdQuery request, CancellationToken ct)
    {
        var incident = await _unitOfWork.Incidents.GetByIdAsync(request.Id, ct);
        if (incident is null)
            throw new NotFoundException("Incident not found");

        return new IncidentDto(incident);
    }
}
