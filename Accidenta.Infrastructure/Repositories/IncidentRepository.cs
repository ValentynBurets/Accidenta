using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Accidenta.Infrastructure.Repositories
{
    public class IncidentRepository : GenericRepository<Incident>, IIncidentRepository
    {
        public IncidentRepository(AccidentaDbContext ctx) : base(ctx) { }

        public new async Task<IEnumerable<Incident>> GetAllAsync(CancellationToken ct)
        {
            return await _ctx.Incidents
                .Include(i => i.Account)
                .ToListAsync(ct);
        }
    }
}
