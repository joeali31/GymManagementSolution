using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _genericRepository;

        public PlanService(IGenericRepository<Plan> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IEnumerable<Plan>> GetAllPlanAsync(bool Tracking = false, CancellationToken ct = default) =>
             await _genericRepository.GetAllAsync(Tracking, ct);
        

        public Task<Plan?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = _genericRepository.GetByIdAsync(id, ct);

            if (plan is null) return null!;

            return plan;
        }
    }
}
