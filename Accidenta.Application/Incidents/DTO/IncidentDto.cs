using Accidenta.Application.Accounts.DTO;
using Accidenta.Domain.Entities;

namespace Accidenta.Application.Incidents.DTO;

public class IncidentDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = default!;
    public AccountDto? Account { get; set; }

    public IncidentDto(Incident incident)
    {
        Id = incident.Id;
        Description = incident.Description;
        Account = incident.Account != null ? new AccountDto(incident.Account) : null;
    }
}
