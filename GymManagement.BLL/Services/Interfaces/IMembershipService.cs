using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.ViewModels.MemberShip;
using GymManagement.BLL.ViewModels.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<Result<IEnumerable<MemberShipViewModel>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<bool>> CreateMembershipAsync(CreateMemberShipViewModel model , CancellationToken ct = default);
        Task<IEnumerable<PlanViewDropDown>> GetPlansDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberViewDropDown>> GetMembersDropDownAsync(CancellationToken ct = default);
        Task<Result<bool>> DeleteMembershipAsync(int memberId , CancellationToken ct = default);
    }
}
