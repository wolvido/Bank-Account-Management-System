using Microsoft.AspNetCore.Mvc;


namespace BmsKhameleon.UI.Controllers
{
    public class HomeController : Controller
    {
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult AccountOverview()
        {
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Calendar()
        {
            return View();
        }

    }
}
