using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Plans;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Class;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {

        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        // GET: PlanController
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            //var Plans = await _genericRepository.GetAllAsync();
            var Plans = await _planService.GetAllPlanAsync(false ,ct);
            return View(Plans);
        }

        // GET: PlanController/Details/5
        public async Task<IActionResult> Details(int id , CancellationToken ct = default)
        {
            var plan = await _planService.GetPlanByIdAsync(id , ct);
            return View(plan);
        }

        // Post: PlanController/Activate/5
        public async Task<IActionResult> Activate(int id , CancellationToken ct = default)
        {
            var plan = await _planService.ActivateAsync(id , ct);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var result = await _planService.GetPlanToUpdateAsync(id , ct);
            if (result is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlaneEditViewModel model, int id , CancellationToken ct = default)
        {
            var result = await _planService.UpdatePlanAsync(model,id, ct);
            return RedirectToAction(nameof(Index));
        }


    }
}
