using Accidenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Application.Incidents.DTO
{
    public class IncidentDto
    {
        public string IncidentName { get; set; }
        public string Description { get; set; } = default!;

        public IncidentDto(Incident incident)
        {
            IncidentName = incident.IncidentName;
            Description = incident.Description;
        }
    }
}
