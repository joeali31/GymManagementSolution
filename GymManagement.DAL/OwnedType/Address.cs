using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.OwnedType
{
    public class Address
    {
        public int BuildingNumber { get; set; } 
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
