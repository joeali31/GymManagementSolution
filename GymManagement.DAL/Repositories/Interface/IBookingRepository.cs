using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interface
{
    public interface IBookingRepository: IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetSessionByIdAsync(int sessionId , CancellationToken ct = default);
    }
}
