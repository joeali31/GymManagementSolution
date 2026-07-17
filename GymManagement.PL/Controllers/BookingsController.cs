using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class BookingsController(IBookingService bookingService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await bookingService.GetAllSessionAsync();
            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var members = await bookingService.GetMembersDropDownAsync(id);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await bookingService.CreateNewBookingAsync(model);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Operation completed successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed";
                }

                return RedirectToAction("GetMembersForUpcomingSession" , new {id = model.SessionId});
            }

            return View(model);
        }

        public async Task<IActionResult> GetMembersForUpcomingSession(int id)
        {
            var result = await bookingService.GetMemberForUpComingBySessionIdAsync(id);

            return View(result.Value);
        }

        public async Task<IActionResult> GetMembersForOngoingSessions(int id)
        {
            var result = await bookingService.GetMemberForOnGoingBySessionIdAsync(id);

            return View(result.Value);
        }

        public async Task<IActionResult> Attended(int memberId , int sessionId)
        {
            var result = await bookingService.MarkAttendedAsync(sessionId ,memberId );

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Operation completed successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Operation failed";
            }

            return RedirectToAction("GetMembersForOngoingSessions", new { id = sessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId , CancellationToken ct = default)
        {
            var result = await bookingService.CancelBookingAsync(sessionId , memberId , ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Operation completed successfully";
            }
            else
            {
                TempData["ErrorMessage"] = $"{result.Erorr}";
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession) , new { id = sessionId });
        }
    }
}
