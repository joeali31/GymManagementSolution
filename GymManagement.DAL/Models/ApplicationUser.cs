using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string firstName { get; set; } = default!;
        public string lastName { get; set; } = default!;
    }
}
