using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct); // ✅ Add this
        Task AddAsync(T ent, CancellationToken ct);
        void Update(T ent);
        void RemoveAsync(T ent, CancellationToken ct);
    }
}
