using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct =  default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId , CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId , CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model , CancellationToken ct = default);
        Task<bool> UpdateMemberAsync(int memberId , MemberToUpdateViewModel model , CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int  memberId , CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int memberId , CancellationToken ct = default);



    }
}
