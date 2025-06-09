using Accidenta.Domain.Interfaces;
using Accidenta.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Accidenta.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AccidentaDbContext _ctx;

        public GenericRepository(AccidentaDbContext ctx) => _ctx = ctx;

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct) =>
            await _ctx.Set<T>().FindAsync(new object[] { id }, ct);

        public async Task<T?> FindAsync(Expression<Func<T, bool>> p, CancellationToken ct) =>
            await _ctx.Set<T>().SingleOrDefaultAsync(p, ct);

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct) => // ✅ Add this
            await _ctx.Set<T>().ToListAsync(ct);

        public async Task AddAsync(T ent, CancellationToken ct) =>
            await _ctx.Set<T>().AddAsync(ent, ct);

        public void Update(T ent) =>
            _ctx.Set<T>().Update(ent);

        public void RemoveAsync(T ent, CancellationToken ct) =>
            _ctx.Set<T>().Remove(ent);
    }
}
