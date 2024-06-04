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

        //TEMPORARILY COMMENTED OUT SO NO ERRORS since we have no model yet
        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult CreateBankAccount()
        //{
        //    return View();
        //}

        //[HttpPost]
        [Route("[action]")]
        public IActionResult CreateBankAccount()
        {
            bool hasErrors = ModelState.Values.Any(v => v.Errors.Count > 0);

            hasErrors = true;
            ViewBag.HasErrors = hasErrors;

            return View();
        }


    }
}
