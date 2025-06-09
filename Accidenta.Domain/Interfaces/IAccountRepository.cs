using Accidenta.Domain.Entities;

namespace Accidenta.Domain.Interfaces;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<Account?> GetByNameAsync(string name, CancellationToken ct);
}
