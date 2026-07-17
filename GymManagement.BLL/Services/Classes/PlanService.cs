using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL;
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
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ActivateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return false;

            var hasMembership = await _unitOfWork.GetRepository<Membership>().AnyAsync(m => m.PlanId == id && m.EndDate > DateTime.Now);
            if(hasMembership) return false; 

            plan.IsActive = !plan.IsActive;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var count = await _unitOfWork.SaveChangesAsync();

            return count > 0;
        }

        public async Task<IEnumerable<Plan>> GetAllPlanAsync(bool Tracking = false, CancellationToken ct = default) =>
             await _unitOfWork.GetRepository<Plan>().GetAllAsync(Tracking, ct);
        

        public async Task<Plan?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan =await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);

            if (plan is null) return null!;

            return plan;
        }

        public async Task<PlaneEditViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);

            if (plan is null) return null!;

            return new PlaneEditViewModel(plan.Id , plan.Name, plan.Description, plan.DurationDays, plan.Price);
        }

        public async Task<bool> UpdatePlanAsync(PlaneEditViewModel model,int id ,CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);

            if (plan is null) return false;

            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            //plan.Name = model.PlanName;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            var count = await _unitOfWork.SaveChangesAsync();

            return count > 0;

        }
    }
}
