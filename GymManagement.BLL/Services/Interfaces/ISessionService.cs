using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.ViewModels.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionDetailsAsync(int SessionId , CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model ,CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int SessionId , CancellationToken ct = default);
        Task<Result<bool>> UpdateSessionAsync(int SessionId , UpdateSessionViewModel model , CancellationToken ct = default);
        Task<Result<bool>> DeleteSessionAsync(int SessionId , CancellationToken ct = default);

        Task<IEnumerable<CategoryViewDropDown>> GetCategoriesDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<TrainerViewDropDown>> GetTrainersDropDownAsync(CancellationToken ct = default);
    }
}
