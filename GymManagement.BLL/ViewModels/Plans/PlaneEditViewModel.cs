using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.Plans
{
    public class PlaneEditViewModel
    {
        public PlaneEditViewModel(int id, string planName, string description, int durationDays, decimal price)
        {
            Id = id;
            PlanName = planName;
            Description = description;
            DurationDays = durationDays;
            Price = price;
        }

        public PlaneEditViewModel() { }

        public string PlanName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
    }
}
