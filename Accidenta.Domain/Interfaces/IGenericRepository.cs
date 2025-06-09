using System.Linq.Expressions;

namespace Accidenta.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);
    Task AddAsync(T ent, CancellationToken ct);
    void Update(T ent);
    void Remove(T ent, CancellationToken ct);

    IQueryable<T> AsQueryable();
}
