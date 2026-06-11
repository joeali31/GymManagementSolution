using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interface
{
    public interface IGenericRepository <TEntity> where TEntity : BaseEntity , new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool Tracking = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> AddAsync(TEntity plan, CancellationToken ct = default);
        Task<int> UpdateAsync(TEntity plan, CancellationToken ct = default);
        Task<int> DeleteAsync(TEntity plan, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity , bool>> Predicate , CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultEntityAsync(Expression<Func<TEntity , bool>> Predicate , CancellationToken ct = default);
    }
}
