using Accidenta.Domain.Entities;

namespace Accidenta.Domain.Interfaces;

public interface IContactRepository : IGenericRepository<Contact>
{
    Task<Contact?> GetByEmailAsync(string email, CancellationToken ct);
}
