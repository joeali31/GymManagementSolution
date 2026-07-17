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
    public class SessionRepository : GenericRepository<Session> , ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Session>> GetAllWithTrainerAndCategoryAsync(CancellationToken ct = default)
        => await _context.sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category).ToListAsync(ct);

        public async Task<IEnumerable<Session>> GetAllWithTrainerAndCategoryAsync(Expression<Func<Session, bool>> Predicate, CancellationToken ct = default)
        {
            return await _context.sessions.AsNoTracking().Where(Predicate).Include(s => s.Trainer).Include(s => s.Category).ToListAsync(ct);
        }

        public async Task<Session?> GetByIdWithTrainerAndCategoryAsync(int id, CancellationToken ct = default)
        {
            return await _context.sessions.Include(s => s.Trainer).Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == id , ct);
        }

        public async Task<int> GetCountOfBookedSlotAsync(int SessionId, CancellationToken ct = default)
        => await _context.bookings.CountAsync(b => b.SessionId == SessionId , ct);
        
    }
}
