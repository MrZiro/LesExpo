using LesExpo.Models;
using LesExpo.Models.ViewModels;
using LesExpo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace LesExpo.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var usersList = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                var userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = roles.FirstOrDefault(),
                    CreatedAt = user.CreatedAt
                };

                usersList.Add(userVM);
            }

            return View(usersList);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();
            var usersList = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                usersList.Add(new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "",
                    CreatedAt = user.CreatedAt
                });
            }

            return Json(new { data = usersList });
        }

        [HttpDelete]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            // Prevent deleting your own account
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == id)
            {
                return Json(new { success = false, message = "Kendi hesabınızı silemezsiniz" });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı" });
            }

            // Check if it's the last admin account
            if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync(SD.Role_Admin);
                if (adminUsers.Count <= 1)
                {
                    return Json(new { success = false, message = "Son admin hesabı silinemez" });
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Kullanıcı başarıyla silindi" });
            }

            return Json(new
            {
                success = false,
                message = "Kullanıcı silinirken hata oluştu: " + result.Errors.FirstOrDefault()?.Description
            });
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(string? id)
        {
            var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();

            RegisterVM registerVM = new()
            {
                RoleList = roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
            };

            // If id is null, we're creating a new user
            if (string.IsNullOrEmpty(id))
            {
                return View(registerVM);
            }

            // Otherwise, we're editing an existing user
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            registerVM.Id = user.Id;
            registerVM.Name = user.Name;
            registerVM.Email = user.Email;
            registerVM.Role = roles.FirstOrDefault();

            return View(registerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(RegisterVM registerVM)
        {
            var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();

            // Populate RoleList in case we need to return the view with errors
            registerVM.RoleList = roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });

            // Update existing user
            if (!string.IsNullOrEmpty(registerVM.Id))
            {
                var user = await _userManager.FindByIdAsync(registerVM.Id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update basic properties
                user.Name = registerVM.Name;
                user.Email = registerVM.Email;
                user.NormalizedEmail = registerVM.Email.ToUpper();
                user.UserName = registerVM.Email;

                // Update user
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Update role if changed
                    var existingRoles = await _userManager.GetRolesAsync(user);
                    var existingRole = existingRoles.FirstOrDefault();

                    if (existingRole != registerVM.Role)
                    {
                        if (!string.IsNullOrEmpty(existingRole))
                        {
                            await _userManager.RemoveFromRoleAsync(user, existingRole);
                        }

                        if (!string.IsNullOrEmpty(registerVM.Role))
                        {
                            await _userManager.AddToRoleAsync(user, registerVM.Role);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, SD.Role_Editor);
                        }
                    }

                    // Update password if provided
                    if (!string.IsNullOrEmpty(registerVM.Password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var passwordResult = await _userManager.ResetPasswordAsync(user, token, registerVM.Password);

                        if (!passwordResult.Succeeded)
                        {
                            foreach (var error in passwordResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            return View(registerVM);
                        }
                    }

                    TempData["success"] = "Kullanıcı başarıyla güncellendi";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registerVM);
            }

            // Create new user
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Editor);
                    }

                    TempData["success"] = "Kullanıcı başarıyla oluşturuldu";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(registerVM);
        }
    }
}
