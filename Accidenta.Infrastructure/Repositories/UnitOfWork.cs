using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccidentaDbContext _ctx;
        public IContactRepository Contacts { get; }
        public IAccountRepository Accounts { get; }
        public IIncidentRepository Incidents { get; }

        public UnitOfWork(
            AccidentaDbContext ctx,
            IContactRepository contacts,
            IAccountRepository accounts,
            IIncidentRepository incidents)
        {
            _ctx = ctx;
            Contacts = contacts;
            Accounts = accounts;
            Incidents = incidents;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct) => _ctx.SaveChangesAsync(ct);
    }
}
