using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OficinaMotos.Domain.Interfaces.Repositories
{
    // Shared CRUD contract for aggregate roots
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(long id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(long id);
        Task SoftDeleteAsync(long id);

        Task<bool> ExistsAsync(long id);
    }
}
