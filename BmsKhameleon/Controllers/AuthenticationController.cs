using BmsKhameleon.Core.Domain.IdentityEntities;
using BmsKhameleon.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BmsKhameleon.UI.Controllers
{
    [Authorize]
    public class AuthenticationController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        private readonly IConfiguration _configuration = configuration;

        [AllowAnonymous]
        [Route("/")]
        [Route("[action]")]
        [HttpGet]
        public IActionResult Authentication()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Authentication(LoginViewModel loginDTO)
        {
            var defaultUserName = _configuration["AccessKeys:DefaultUserKey"];
            var defaultAccessKey = _configuration["AccessKeys:DefaultKey"];
            if (defaultAccessKey == loginDTO.Password && defaultUserName == loginDTO.Username)
            {
                return await HandleDefaultUser(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, loginDTO.KeepLoggedIn, true);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("LoginError", "Invalid email or password");
            return View();
        }

        [AllowAnonymous]
        private async Task<IActionResult> HandleDefaultUser(LoginViewModel loginDTO) {

            var user = await _userManager.FindByNameAsync(loginDTO.Username);
            if (user == null)
            {
                ApplicationUser defaultUser = new ApplicationUser
                {
                    UserName = loginDTO.Username
                };

                var createResult = await _userManager.CreateAsync(defaultUser, loginDTO.Password);

                if(!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("LoginError", error.Description);
                    }
                    return View("Authentication", loginDTO);
                }


                //check if role exists, create if not
                var roleExists = await _roleManager.RoleExistsAsync("Admin");
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
                }

                await _userManager.AddToRoleAsync(defaultUser, "Admin");
                await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, loginDTO.KeepLoggedIn, true);
                return RedirectToAction("Admin", "Admin");
            }

            await _signInManager.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, loginDTO.KeepLoggedIn, true);
            return RedirectToAction("Admin", "Admin");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Authentication", "Authentication");
        }

    }
}
