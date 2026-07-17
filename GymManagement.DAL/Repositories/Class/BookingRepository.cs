using GymManagement.DAL.DbContexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Class
{
    public class BookingRepository(GymDbContext context) : GenericRepository<Booking>(context), IBookingRepository
    {
        public async Task<IEnumerable<Booking>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        => await context.bookings.AsNoTracking().Include(b => b.Member).Where(b => b.SessionId == sessionId).ToListAsync(ct);
    }
}
