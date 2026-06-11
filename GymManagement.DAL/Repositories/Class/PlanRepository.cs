using GymManagement.DAL.DbContexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Class
{
    public class PlanRepository : GenericRepository<Plan> , IPlanRepository
    {
        private readonly GymDbContext _context;

        public PlanRepository(GymDbContext context) : base(context) 
        {
            _context = context;
        }


        //public async Task<IEnumerable<Plan>> GetAllAsync(bool Tracking = false, CancellationToken ct = default)
        //=> Tracking ? await _context.plans.ToListAsync() : await _context.plans.AsNoTracking().ToListAsync(ct);
        

        //public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        //=> await _context.plans.FirstOrDefaultAsync(p => p.Id == id , ct);
        

        //public Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
