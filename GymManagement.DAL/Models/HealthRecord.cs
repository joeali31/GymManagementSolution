using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; }

        // Relationship

        public Member Member { get; set; } 
        public int MemberId { get; set; }

    }
}
