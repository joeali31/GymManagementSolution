using GymManagement.DAL.DbContexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Class
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(GymDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool Tracking = false, CancellationToken ct = default) =>
             Tracking ? await _dbset.ToListAsync(ct) : await _dbset.AsNoTracking().ToListAsync(ct);
        

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default) =>
             await _dbset.FirstOrDefaultAsync(x => x.Id == id , ct);
        

        public void Add(TEntity model)
        {
            _dbset.Add(model);
        }

        public void Update(TEntity model)
        {
            _dbset.Update(model);
        }

        public void Delete(TEntity model)
        {
            _dbset.Remove(model);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken ct = default)
        {
            return await _dbset.AnyAsync(Predicate ,ct);
        }

        public async Task<TEntity?> FirstOrDefaultEntityAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken ct = default)
        {
            return await _dbset.FirstOrDefaultAsync(Predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> Predicat, bool Tracking = false, CancellationToken ct = default)
        => Tracking ? await _dbset.Where(Predicat).ToListAsync(ct) : await _dbset.AsNoTracking().Where(Predicat).ToListAsync(ct);

        public async Task<int> CountEntityAsync(Expression<Func<TEntity, bool>>? Predicate, CancellationToken ct = default)
        {
            return Predicate == null
                ? await _dbset.AsNoTracking().CountAsync(ct)
                : await _dbset.AsNoTracking().CountAsync(Predicate, ct);
        }
    }
}
