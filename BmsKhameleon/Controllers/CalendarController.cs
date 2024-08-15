using Microsoft.AspNetCore.Mvc;

namespace BmsKhameleon.UI.Controllers
{
    public class CalendarController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
