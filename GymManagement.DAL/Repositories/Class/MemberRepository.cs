using GymManagement.DAL.DbContexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Class
{
    public class MemberRepository : GenericRepository<Member> , IMemberRepository
    {
        private readonly GymDbContext _context;
        public MemberRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
