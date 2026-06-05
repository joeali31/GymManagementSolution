using GymManagement.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class Trainer : GymUser
    {
        public TrainerSpecialty Specialty { get; set; }

        // Relationship

        public ICollection<Session> Sessions { get; set; } = new List<Session>();

    }
}
