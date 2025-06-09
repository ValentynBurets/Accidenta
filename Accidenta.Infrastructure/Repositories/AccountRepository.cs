using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accidenta.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AccidentaDbContext ctx) : base(ctx) { }

        public new async Task<IEnumerable<Account>> GetAllAsync(CancellationToken ct)
        {
            return await _ctx.Accounts
                .Include(a => a.Contact) // Include contact if needed
                .ToListAsync(ct);
        }

        public Task<Account?> GetByNameAsync(string name, CancellationToken ct) =>
            _ctx.Accounts
                .Include(a => a.Contact)
                .SingleOrDefaultAsync(a => a.Name == name, ct);
    }
}
