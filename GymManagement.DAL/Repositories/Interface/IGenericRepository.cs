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
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity , bool>> Predicat ,bool Tracking = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        void Add(TEntity plan);
        void Update(TEntity plan);
        void Delete(TEntity plan);
        Task<bool> AnyAsync(Expression<Func<TEntity , bool>> Predicate , CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultEntityAsync(Expression<Func<TEntity , bool>> Predicate , CancellationToken ct = default);
        Task<int> CountEntityAsync(Expression<Func<TEntity , bool>>? Predicate , CancellationToken ct = default);
    }
}
