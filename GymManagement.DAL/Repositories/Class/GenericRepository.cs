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
        

        public async Task<int> AddAsync(TEntity model, CancellationToken ct = default)
        {
            await _dbset.AddAsync(model, ct);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> UpdateAsync(TEntity model, CancellationToken ct = default)
        {
            _dbset.Update(model);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(TEntity model, CancellationToken ct = default)
        {
            _dbset.Remove(model);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken ct = default)
        {
            return await _dbset.AnyAsync(Predicate ,ct);
        }

        public async Task<TEntity?> FirstOrDefaultEntityAsync(Expression<Func<TEntity, bool>> Predicate, CancellationToken ct = default)
        {
            return await _dbset.FirstOrDefaultAsync(Predicate, ct);
        }
    }
}
