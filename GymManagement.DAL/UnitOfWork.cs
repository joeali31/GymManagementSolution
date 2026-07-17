using GymManagement.DAL.DbContexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Class;
using GymManagement.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly Dictionary<string, object> _repositories = [];
        private readonly ISessionRepository _sessionRepository;

        public UnitOfWork(GymDbContext context , ISessionRepository sessionRepository)
        {
            _context = context;
            _sessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository => _sessionRepository;

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var objectType = typeof(TEntity).Name;

            if (_repositories.TryGetValue(objectType , out var value))
                return value as IGenericRepository<TEntity>;

            var newRepo = new GenericRepository<TEntity>(_context);
            _repositories.Add(objectType, newRepo);

            return newRepo;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        
    }
}
