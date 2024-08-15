using Microsoft.AspNetCore.Mvc;

namespace BmsKhameleon.UI.Controllers
{
    public class AccountOverviewController : Controller
    {
        [Route("[action]")]
        public IActionResult AccountOverview()
        {
            return View();
        }
    }
}
