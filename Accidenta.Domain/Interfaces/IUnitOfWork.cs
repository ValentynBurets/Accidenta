using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IContactRepository Contacts { get; }
        IAccountRepository Accounts { get; }
        IIncidentRepository Incidents { get; }
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
