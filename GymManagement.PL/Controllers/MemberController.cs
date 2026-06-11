using GymManagement.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var Members = await _memberService.GetAllMembersAsync();

            return View(Members);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model , CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {

                var result = await _memberService.CreateMemberAsync(model, ct);

                if (result)
                {
                    TempData["SuccessMessage"] = "Operation completed successfully";
                }
                else
                {
                    TempData["ErorrMessage"] = "Operation failed";
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> MemberDetails(int id , CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberDetailsAsync(id , ct);

            if (result is null)
            {
                TempData["ErorrMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View(result);
        }

        public async Task<IActionResult> HealthRecordDetails(int id , CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberHealthRecordAsync(id , ct);

            if (result is null)
            {
                TempData["ErorrMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> EditMember(int id , CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberToUpdateAsync(id , ct);

            if (result is null)
            {
                TempData["ErorrMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> EditMember(int id , MemberToUpdateViewModel model , CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberAsync(id , model , ct);

                if (result)
                {
                    TempData["SuccessMessage"] = "Operation completed successfully";
                }
                else
                {
                    TempData["ErorrMessage"] = "Operation failed";
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }


        public async Task<IActionResult> Delete(int id , CancellationToken ct = default)
        {
            var result = await _memberService.GetMemberDetailsAsync(id , ct);

            if (result is null)
            {
                TempData["ErorrMessage"] = "Operation failed";

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct = default)
        {
            var result = await _memberService.DeleteMemberAsync(id , ct);

            if (result)
            {
                TempData["SuccessMessage"] = "Operation completed successfully";
            }
            else
            {
                TempData["ErorrMessage"] = "Operation failed";
            }

            return RedirectToAction("Index");
        }



    }
}
