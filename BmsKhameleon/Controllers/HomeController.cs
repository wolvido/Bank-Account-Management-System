using BmsKhameleon.Core.DTO.AccountDTOs;
using BmsKhameleon.Core.ServiceContracts;
using BmsKhameleon.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace BmsKhameleon.UI.Controllers
{
    public class HomeController(IAccountsService accountsService) : Controller
    {
        private readonly IAccountsService _accountsService = accountsService;

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<AccountResponse> accounts = await _accountsService.GetAllAccounts();

            AccountViewModel accountViewModel = new AccountViewModel
            {
                AccountResponses = accounts
            };

            return View(accountViewModel);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateBankAccount(AccountCreateRequest? accountCreateRequest)
        {
            if(accountCreateRequest == null)
            {
                return BadRequest("Invalid account request.");
            }

            bool result = await _accountsService.CreateAccount(accountCreateRequest);

            if (result == false)
            {
                return BadRequest("Unable to create the account. Please try again.");
            }

            return RedirectToAction("Index");
        }

    }
}
