using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Home;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalyticsService(IUnitOfWork unitOfWork) : IAnalyticsService
    {
        public async Task<AnalyticsViewModel> GetDataAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var upComingSession = await unitOfWork.GetRepository<Session>().CountEntityAsync(s => s.StartDate > now, ct);
            var onGoingSession = await unitOfWork.GetRepository<Session>().CountEntityAsync(s => s.StartDate <= now && s.EndDate >= now, ct);
            var completedSession = await unitOfWork.GetRepository<Session>().CountEntityAsync(s => s.EndDate < now, ct);
            var totalTrainers = await unitOfWork.GetRepository<Trainer>().CountEntityAsync(null , ct);
            var totalMembers = await unitOfWork.GetRepository<Member>().CountEntityAsync( null,ct);
            var activeMembers = await unitOfWork.GetRepository<Membership>().CountEntityAsync(m => m.EndDate > now , ct);

            return new AnalyticsViewModel(totalMembers , activeMembers , totalTrainers , upComingSession , onGoingSession , completedSession);
        }
    }
}
