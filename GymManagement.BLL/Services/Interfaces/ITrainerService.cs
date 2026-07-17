using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.ViewModels.Trainer;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public  interface ITrainerService
    {
        Task<Result<IEnumerable<TrainerViewModel>>> GetAllTrainerAsync(CancellationToken ct = default);
        Task<Result<bool>> CreateTrainerAsync(CreateTrainerViewModel model ,CancellationToken ct = default);
        Task<Result<TrainerViewModel?>> GetTrainerDetailsAsync(int id, CancellationToken ct = default);

        Task<Result<UpdateTrainerViewModel?>> GetTrainerToUpdateAsync(int id, CancellationToken ct = default);
        Task<Result<bool>> UpdateTrainerAsync(int id, UpdateTrainerViewModel model, CancellationToken ct = default);
        Task<Result<bool>> DeleteTrainerAsync(int id, CancellationToken ct = default);
    }
}
