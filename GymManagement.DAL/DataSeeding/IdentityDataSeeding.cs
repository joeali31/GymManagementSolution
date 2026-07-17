using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymManagement.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(
                        UserManager<ApplicationUser> userManager,
                        RoleManager<IdentityRole> roleManager,
                        ILogger logger)
        {
            try
            {
                var hasUser = await userManager.Users.AnyAsync();
                var hasRole = await roleManager.Roles.AnyAsync();

                if (hasUser && hasRole) return;

                if (!hasRole)
                {
                    var newRoles = new List<IdentityRole>()
                {
                    new("SuperAdmin"),
                    new("Admin")
                };

                    foreach (var role in newRoles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }

                }

                if (!hasUser)
                {
                    var SuperAdmin = new ApplicationUser()
                    {
                        firstName = "Yousef",
                        lastName = "Ali",
                        UserName = "YousefAli",
                        Email = "ya91475@gmail.com",
                        PhoneNumber = "01287566947"
                    };

                    var result = await userManager.CreateAsync(SuperAdmin, "P@ssw0rd");

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError("Code: {Code}, Description: {Description}",
                                error.Code,
                                error.Description);
                        }
                    }

                    await userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");

                    var Admin = new ApplicationUser()
                    {
                        firstName = "Ahmed",
                        lastName = "Ali",
                        UserName = "AhmedAli",
                        Email = "Ahmed475@gmail.com",
                        PhoneNumber = "01287566947"
                    };

                    await userManager.CreateAsync(Admin, "P@ssw0rd");

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError("Code: {Code}, Description: {Description}",
                                error.Code,
                                error.Description);
                        }
                    }

                    await userManager.AddToRoleAsync(Admin, "Admin");

                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return;
            }
        }
    }
}
