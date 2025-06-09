using Accidenta.Domain.Entities;
using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Infrastructure.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(AccidentaDbContext ctx) : base(ctx) { }
        public Task<Contact?> GetByEmailAsync(string email, CancellationToken ct) =>
            _ctx.Contacts.SingleOrDefaultAsync(c => c.Email == email, ct);

        public new async Task<IEnumerable<Contact>> GetAllAsync(CancellationToken ct)
        {
            return await _ctx.Contacts
                             .OrderBy(c => c.LastName)
                             .ThenBy(c => c.FirstName)
                             .ToListAsync(ct);
        }
    }
}
