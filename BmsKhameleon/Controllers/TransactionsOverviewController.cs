using Microsoft.AspNetCore.Mvc;

namespace BmsKhameleon.UI.Controllers
{
    public class TransactionsOverviewController : Controller
    {
        [Route("[action]/{accountId}/{date}")]
        public IActionResult TransactionsOverview()
        {
            return View();
        }
    }
}
