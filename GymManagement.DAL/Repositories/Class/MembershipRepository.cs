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
    public class MembershipRepository(GymDbContext context) : GenericRepository<Membership>(context), IMembershipRepository
    {
        public async Task<IEnumerable<Membership>> GetMembershipsWithPlanAndMemberAsync(CancellationToken ct = default)
        => await context.memberships.AsNoTracking().Where(m => m.EndDate > DateTime.Now).Include(m => m.Plan).Include(m => m.Member).ToListAsync(ct);
        
        public async Task<Membership?> GetMembershipsWithPlanAndMemberAsync(Expression<Func<Membership, bool>> Predicate , CancellationToken ct = default)
        => await context.memberships.AsNoTracking().Include(m => m.Plan).Include(m => m.Member).FirstOrDefaultAsync(Predicate);
    }
}
