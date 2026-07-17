using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.ViewModels.Booking;
using GymManagement.BLL.ViewModels.MemberShip;
using GymManagement.BLL.ViewModels.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForUpComingBySessionIdAsync(int sessionId , CancellationToken ct = default);
        Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForOnGoingBySessionIdAsync(int sessionId , CancellationToken ct = default);
        Task<Result<bool>> CreateNewBookingAsync(CreateBookingViewModel model , CancellationToken ct = default);
        Task<IEnumerable<MemberViewDropDown>> GetMembersDropDownAsync(int sessionId, CancellationToken ct = default);
        Task<Result<bool>> CancelBookingAsync(int sessionId, int memberId ,CancellationToken ct = default);
        Task<Result<bool>> MarkAttendedAsync(int sessionId, int memberId ,CancellationToken ct = default);

    }
}
