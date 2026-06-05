using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Membership : BaseEntity
    {
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; }

        // SatrtDate --> CreatedAt
        //public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Drived Attribute
        // Any Attribute is just to get value does not mapped in database
        public bool IsActive => EndDate > DateTime.UtcNow;
        public string Status => EndDate > DateTime.UtcNow ? "Active" : "Expired";

    }
}
