using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Class;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
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
    }
}
