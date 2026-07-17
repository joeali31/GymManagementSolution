using AutoMapper;
using GymManagement.BLL.ResultBattern;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Booking;
using GymManagement.BLL.ViewModels.MemberShip;
using GymManagement.BLL.ViewModels.Session;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class BookingService(IUnitOfWork unitOfWork , IMapper mapper , IBookingRepository bookingRepository) : IBookingService
    {
        public async Task<Result<bool>> CancelBookingAsync(int sessionId, int memberId, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId);
            if (session == null) return Result<bool>.NotFound("Can not Fount Session!");
            if(session.StartDate <= DateTime.Now) return Result<bool>.Failed("Can not Cancel booking For a Session has already started ");
            
            var booking = await unitOfWork.GetRepository<Booking>()
                .FirstOrDefaultEntityAsync(b => b.SessionId == sessionId && b.MemberId == memberId);
            if (booking is null) return Result<bool>.NotFound("Can not Fount Booking!");
            unitOfWork.GetRepository<Booking>().Delete(booking);
            var count = await unitOfWork.SaveChangesAsync();
            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Cancel Booking !");
        }

        public async Task<Result<bool>> CreateNewBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetByIdAsync(model.SessionId);
            if (session == null) return Result<bool>.NotFound("Can not Fount Session!");
            if (session.StartDate <= DateTime.Now) return Result<bool>.Failed("Can not Cancel booking For a Session has already started ");

            var hasActiveMembership = await unitOfWork.GetRepository<Membership>()
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now);
            
            if(!hasActiveMembership)
                return Result<bool>.Failed("member does not have an active membership");

            var alreadyBooked = await unitOfWork.GetRepository<Booking>()
                .AnyAsync(b => b.MemberId == model.MemberId && model.SessionId == model.SessionId);

            if (!hasActiveMembership)
                return Result<bool>.Failed("member had already booked");

            var booked = await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(model.SessionId);
            if (booked >= session.Capacity)
                return Result<bool>.Failed("Session id full");

            unitOfWork.GetRepository<Booking>().Add(new Booking()
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId
            });

            var count = await unitOfWork.SaveChangesAsync();
            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Create Booking !");
        }

        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionAsync(CancellationToken ct = default)
        {
            var bookings = await unitOfWork.SessionRepository.GetAllWithTrainerAndCategoryAsync(s => s.EndDate > DateTime.UtcNow , ct);
            if (!bookings.Any()) return Result<IEnumerable<SessionViewModel>>.Failed("There is no session");
            var mappedSession = mapper.Map<IEnumerable<SessionViewModel>>(bookings);
            foreach (var item in mappedSession)
            {
                item.AvailableSlots = item.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(item.Id);
            }

            return Result<IEnumerable<SessionViewModel>>.Ok(mappedSession);
        }

        public async Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForOnGoingBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await bookingRepository.GetSessionByIdAsync(sessionId);

            var bookingsSelected = bookings.Select(b => new MemberForSessionViewModel()
            {
                MemberId = b.MemberId,
                MemberName = b.Member.Name,
                SessionId = b.SessionId,
                BookingDate = b.CreatedAt.ToString(),
                IsAttended = b.IsAttended

            }).ToList();

            return Result<IEnumerable<MemberForSessionViewModel>>.Ok(bookingsSelected);
        }

        public async Task<Result<IEnumerable<MemberForSessionViewModel>>> GetMemberForUpComingBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await bookingRepository.GetSessionByIdAsync(sessionId);

            var bookingsSelected = bookings.Select(b => new MemberForSessionViewModel()
            {
                MemberId = b.MemberId,
                MemberName = b.Member.Name,
                SessionId = b.SessionId,
                BookingDate = b.CreatedAt.ToString()

            }).ToList();

            return Result<IEnumerable<MemberForSessionViewModel>>.Ok(bookingsSelected);
        }

        public async Task<IEnumerable<MemberViewDropDown>> GetMembersDropDownAsync(int sessionId, CancellationToken ct = default)
        {
            var bookings = await unitOfWork.GetRepository<Booking>().GetAllAsync(b => b.SessionId == sessionId);
            var bookedMemberIds = bookings.Select(b => b.MemberId);
            var avilableMembers = await unitOfWork.GetRepository<Member>().GetAllAsync(m => !bookedMemberIds.Contains(m.Id));
            var mappedMembers = mapper.Map<IEnumerable<MemberViewDropDown>>(avilableMembers);
            return mappedMembers;
        }

        public async Task<Result<bool>> MarkAttendedAsync(int sessionId, int memberId, CancellationToken ct = default)
        {
            var booking = await unitOfWork.GetRepository<Booking>()
                .FirstOrDefaultEntityAsync(b => b.SessionId == sessionId && b.MemberId == memberId);
            if (booking is null) return Result<bool>.NotFound("Can not Fount Booking!");

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.UtcNow;
            unitOfWork.GetRepository<Booking>().Update(booking);
            var count = await unitOfWork.SaveChangesAsync();
            return count > 0 ?
                Result<bool>.Ok(true)
                :
                Result<bool>.Failed("Failed to Market Booking !");
        }
    }
}
