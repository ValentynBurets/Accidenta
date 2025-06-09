using Accidenta.Domain.Entities;

namespace Accidenta.Application.Incidents.DTO;

public class IncidentDto
{
    public string Id { get; set; }
    public string Description { get; set; } = default!;

    public IncidentDto(Incident incident)
    {
        Id = incident.Id;
        Description = incident.Description;
    }
}
