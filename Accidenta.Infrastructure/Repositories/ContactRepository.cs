using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Accidenta.Infrastructure.Repositories;

public class ContactRepository : GenericRepository<Contact>, IContactRepository
{
    public ContactRepository(AccidentaDbContext ctx) : base(ctx) { }
    public Task<Contact?> GetByEmailAsync(string email, CancellationToken ct) =>
        _ctx.Contacts.FirstOrDefaultAsync(c => c.Email == email, ct);

    public new async Task<IEnumerable<Contact>> GetAllAsync(CancellationToken ct) =>
        await _ctx.Contacts
                  .OrderBy(c => c.LastName)
                  .ThenBy(c => c.FirstName)
                  .ToListAsync(ct);
}
