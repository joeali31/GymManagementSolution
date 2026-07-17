using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberShip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class MemberShipsController(IMembershipService membershipService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var result = await membershipService.GetAllAsync(ct);

            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct = default)
        {
            ViewBag.Members = new SelectList(await membershipService.GetMembersDropDownAsync(ct), "Id", "Name");
            ViewBag.Plans = new SelectList(await membershipService.GetPlansDropDownAsync(ct), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model , CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await membershipService.CreateMembershipAsync(model, ct);
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
            ViewBag.Members = new SelectList(await membershipService.GetMembersDropDownAsync(ct), "Id", "Name");
            ViewBag.Plans = new SelectList(await membershipService.GetPlansDropDownAsync(ct), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Cancel(int id , CancellationToken ct = default)
        {
            var result = await membershipService.DeleteMembershipAsync(id, ct);

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
