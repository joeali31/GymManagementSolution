using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Plan : BaseEntity
    {
        // Defult! remove the warnning
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        // Relationship

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    }
}
