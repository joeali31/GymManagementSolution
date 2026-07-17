using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<Plan>> GetAllPlanAsync( bool Tracking = false ,CancellationToken ct = default);
        Task<Plan?> GetPlanByIdAsync(int id , CancellationToken ct = default);
        Task<bool> ActivateAsync(int id , CancellationToken ct = default);
        Task<PlaneEditViewModel?> GetPlanToUpdateAsync(int id , CancellationToken ct = default);
        Task<bool> UpdatePlanAsync(PlaneEditViewModel model , int id, CancellationToken ct = default);
    }
}
