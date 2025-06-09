using Accidenta.Application.Incidents.DTO;
using Accidenta.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Incidents.Queries
{
    public record GetAllIncidentsQuery() : IRequest<IEnumerable<IncidentDto>>;

    public class GetAllIncidentsQueryHandler : IRequestHandler<GetAllIncidentsQuery, IEnumerable<IncidentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllIncidentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IncidentDto>> Handle(GetAllIncidentsQuery request, CancellationToken ct)
        {
            var incidents = await _unitOfWork.Incidents.GetAllAsync(ct);
            return incidents.Select(i => new IncidentDto(i));
        }
    }
}
