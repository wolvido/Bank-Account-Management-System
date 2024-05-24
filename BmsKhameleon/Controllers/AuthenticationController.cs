using Microsoft.AspNetCore.Mvc;

namespace BmsKhameleon.UI.Controllers
{
    public class AuthenticationController : Controller
    {
        [Route("/")]
        public IActionResult Authentication()
        {
            return View();
        }
    }
}
