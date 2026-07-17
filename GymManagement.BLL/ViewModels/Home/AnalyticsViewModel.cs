using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.Home
{
    public class AnalyticsViewModel
    {
        public AnalyticsViewModel(int totalMembers, int activeMembers, int totalTrainers, int upcomingSessions, int ongoingSessions, int completedSessions)
        {
            TotalMembers = totalMembers;
            ActiveMembers = activeMembers;
            TotalTrainers = totalTrainers;
            UpcomingSessions = upcomingSessions;
            OngoingSessions = ongoingSessions;
            CompletedSessions = completedSessions;
        }

        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int TotalTrainers { get; set; }
        public int UpcomingSessions { get; set; }
        public int OngoingSessions { get; set; }
        public int CompletedSessions { get; set; }
    }
}
