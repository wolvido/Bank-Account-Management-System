using BmsKhameleon.Core.Domain.IdentityEntities;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BmsKhameleon.UI.Controllers
{
    public class AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        [HttpGet]
        [Route("[action]")]
        public IActionResult Admin()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerDTO)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerDTO.UserName
            };

            //check if the role exists, create if not
            var existingRole = await _roleManager.RoleExistsAsync(registerDTO.UserRole.ToString());
            if(!existingRole)
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = registerDTO.UserRole.ToString() });
            }

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registerDTO.UserRole.ToString());
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("Admin", registerDTO);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> UserRolesTable()
        {
            // Load all users into memory
            var users = await _userManager.Users.ToListAsync();

            // Fetch roles asynchronously for all users
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    User = user,
                    UserRole = roles.FirstOrDefault() ?? "No Role"
                });
            }

            return PartialView("~/Views/Shared/UsersTable/_UsersTable.cshtml", userRoles);
        }

    }
}
