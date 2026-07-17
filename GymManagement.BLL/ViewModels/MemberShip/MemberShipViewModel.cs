using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberShip
{
    public class MemberShipViewModel
    {
        public int MemberId { get; set; }
        public int planId { get; set; }
        public string MemberName { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
