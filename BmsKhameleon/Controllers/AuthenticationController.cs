using BmsKhameleon.Core.Domain.IdentityEntities;
using BmsKhameleon.UI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BmsKhameleon.UI.Controllers
{
    public class AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        private readonly IConfiguration _configuration = configuration;

        [Route("/")]
        [Route("[action]")]
        [HttpGet]
        public IActionResult Authentication()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Authentication(LoginViewModel LoginDTO)
        {
            var defaultUserName = _configuration["AccessKeys:DefaultUserKey"];
            var defaultAccessKey = _configuration["AccessKeys:DefaultKey"];
            if (defaultAccessKey == LoginDTO.Password && defaultUserName == LoginDTO.Username)
            {
                return RedirectToAction("Admin", "Admin");
            }

            var result = _signInManager.PasswordSignInAsync(LoginDTO.Username, LoginDTO.Password, LoginDTO.KeepLoggedIn, true).Result;

            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("LoginError", "Invalid email or password");
            return View();

        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Authentication");
        }

    }
}
