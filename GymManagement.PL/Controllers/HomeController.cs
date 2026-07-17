using System.Diagnostics;
using System.Threading.Tasks;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.PL.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalyticsService _analyticsService;

        public HomeController(
            ILogger<HomeController> logger,
            IAnalyticsService analyticsService)
        {
            _logger = logger;
            _analyticsService = analyticsService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _analyticsService.GetDataAsync();
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
