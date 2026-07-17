using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interface
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        Task<IEnumerable<Membership>> GetMembershipsWithPlanAndMemberAsync(CancellationToken ct = default);
        Task<Membership> GetMembershipsWithPlanAndMemberAsync(Expression<Func<Membership, bool>> Predicate, CancellationToken ct = default);
    }
}
