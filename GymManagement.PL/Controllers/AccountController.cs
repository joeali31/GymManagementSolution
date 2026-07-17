using GymManagement.BLL.ViewModels.Account;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager) : Controller
    {

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model , CancellationToken ct = default)
        {
            if (ModelState.IsValid)
            {
                // check Email
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user is null)
                {
                    ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                    return View(model);
                }

                // check password
                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index" ,"Home" );
                }
                else
                {
                    ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                    return View(model);
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
        
    }
}
