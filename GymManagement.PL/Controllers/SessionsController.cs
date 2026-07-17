using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var result = await _sessionService.GetAllSessionsAsync(ct);

            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersDropDownAsync(ct) , "Id" , "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesDropDownAsync(ct), "Id" , "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model , CancellationToken ct = default)
        {

            if (ModelState.IsValid)
            {

                var result = await _sessionService.CreateSessionAsync(model, ct);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Operation completed successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Erorr;
                }

                return RedirectToAction("Index");
            }

            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersDropDownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesDropDownAsync(ct), "Id", "Name");

            return View(model);

        }
    
        public async Task<IActionResult> Details(int id , CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionDetailsAsync(id, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View(result.Value);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken ct =default)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersDropDownAsync(ct), "Id" , "Name");
            return View(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , UpdateSessionViewModel model, CancellationToken ct =default)
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.UpdateSessionAsync(id, model, ct);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Operation completed successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Operation failed";
                }

                return RedirectToAction("Index");
            }

            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersDropDownAsync(ct), "Id", "Name");
            return View(model);
        }

        
        public async Task<IActionResult> Delete(int id , CancellationToken ct =default)
        {
            var result = await _sessionService.GetSessionDetailsAsync(id, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.DeleteSessionAsync(id, ct);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Operation completed successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Operation failed";
            }

            return RedirectToAction("Index");
        }

    }
}
