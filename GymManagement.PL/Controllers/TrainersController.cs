using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Trainer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class TrainersController(ITrainerService trainerService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var result = await trainerService.GetAllTrainerAsync(ct);

            return View(result.Value);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model , CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await trainerService.CreateTrainerAsync(model, ct);

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
            return View(model);
        }

        public async Task<IActionResult> Details(int id , CancellationToken ct = default)
        {
            var result = await trainerService.GetTrainerDetailsAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View(result.Value);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var result = await trainerService.GetTrainerToUpdateAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , UpdateTrainerViewModel model , CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await trainerService.UpdateTrainerAsync(id, model, ct);

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

            return View(model);
        }


        public async Task<IActionResult> Delete(int id , CancellationToken ct = default)
        {
            var result = await trainerService.GetTrainerDetailsAsync(id, ct);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct = default)
        {

            var result = await trainerService.DeleteTrainerAsync(id, ct);

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
