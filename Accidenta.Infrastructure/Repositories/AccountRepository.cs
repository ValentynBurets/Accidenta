using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Accidenta.Infrastructure.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(AccidentaDbContext ctx) : base(ctx) { }

    public async new Task<IEnumerable<Account>> GetAllAsync(CancellationToken ct) =>
        await _ctx.Accounts
                  .Include(a => a.Contact)
                  .ToListAsync(ct);

    public async Task<Account?> GetByNameAsync(string name, CancellationToken ct) =>
        await _ctx.Accounts
                  .Include(a => a.Contact)
                  .FirstOrDefaultAsync(a => a.Name == name, ct);
}
