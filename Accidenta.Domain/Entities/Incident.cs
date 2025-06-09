using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Domain.Entities
{
    public class Incident
    {
        public string IncidentName { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = null!;
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}
