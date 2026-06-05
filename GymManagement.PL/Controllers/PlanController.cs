using GymManagement.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class PlanController : Controller
    {

        private readonly IPlanRepository _planRepository;

        public PlanController(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        // GET: PlanController
        public async Task<ActionResult> Index(CancellationToken ct = default)
        {
            var Plans = await _planRepository.GetAllAsync(ct: ct);
            return View(Plans);
        }

        // GET: PlanController/Details/5
        public async Task<ActionResult> Details(int id , CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id , ct);
            return View(plan);
        }
    }
}
