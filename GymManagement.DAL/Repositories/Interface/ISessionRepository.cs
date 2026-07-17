using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interface
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllWithTrainerAndCategoryAsync(CancellationToken ct = default);
        Task<IEnumerable<Session>> GetAllWithTrainerAndCategoryAsync(Expression<Func<Session, bool>> Predicate, CancellationToken ct = default);
        Task<Session?> GetByIdWithTrainerAndCategoryAsync(int id , CancellationToken ct = default);
        Task<int> GetCountOfBookedSlotAsync(int SessionId, CancellationToken ct = default);
    }
}
