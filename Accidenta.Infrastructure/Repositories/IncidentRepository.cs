using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Accidenta.Infrastructure.Repositories;

public class IncidentRepository : GenericRepository<Incident>, IIncidentRepository
{
    public IncidentRepository(AccidentaDbContext ctx) : base(ctx) { }

    public new async Task<IEnumerable<Incident>> GetAllAsync(CancellationToken ct) =>
        await _ctx.Incidents
                  .Include(i => i.Account)
                  .ToListAsync(ct);
}
