using BmsKhameleon.Core.DTO.AccountDTOs;
using BmsKhameleon.Core.ServiceContracts;
using BmsKhameleon.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BmsKhameleon.UI.Controllers
{
    public class HomeController(IAccountsService accountsService) : Controller
    {
        private readonly IAccountsService _accountsService = accountsService;

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index(string? bankFilter, string? searchString)
        {
            var banks = await _accountsService.GetAllAccountBanks();
            banks.Insert(0, "Any Bank");

            ViewBag.Banks = banks;

            List<AccountResponse> accounts = await _accountsService.GetAllAccounts();

            //use search if searchString is not null
            if (searchString != null)
            {
                if (bankFilter == "Any Bank")
                {
                    bankFilter = null;
                }

                accounts = await _accountsService.GetFilteredAccounts(searchString, bankFilter);
            }

            //filter banks if bankFilter is not null
            if (bankFilter != null && bankFilter != "Any Bank")
            {
                accounts = accounts.Where(account => account.BankName == bankFilter).ToList();
                banks.Remove(bankFilter);
                banks.Insert(0, bankFilter);
            }

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
            if (accountCreateRequest == null)
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

        [Route("[action]/{accountId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteBankAccount(Guid accountId)
        {
            var existingAccount = await _accountsService.GetAccountById(accountId);
            if (existingAccount == null)
            {
                return BadRequest("Account already deleted or not found");
            }

            bool result = await _accountsService.DeleteAccount(accountId);

            if (result == false)
            {
                return BadRequest("Unable to delete the account. Please try again.");
            }

            return RedirectToAction("Index");
        }

        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateBankAccount(AccountUpdateRequest? accountUpdateRequest)
        {
            if (accountUpdateRequest == null)
            {
                return BadRequest("Invalid account request.");
            }

            bool result = await _accountsService.UpdateAccount(accountUpdateRequest);

            if (result == false)
            {
                return BadRequest("Unable to update the account. Please contact site manager.");
            }

            return RedirectToAction("Index");
        }

        [Route("[action]/{accountId}")]
        [HttpGet]
        public async Task<IActionResult> UpdateBankAccountPartial(Guid accountId)
        {

            AccountResponse? account = await _accountsService.GetAccountById(accountId);

            if (account == null)
            {
                return BadRequest("Account not found.");
            }

            if (account.DateEnrolled == null)
            {
                throw new Exception("Date enrolled is null.");
            }

            var dateEnrolled = account.DateEnrolled.Value;
            var dateEnrolledFormatted = dateEnrolled.ToString("MMMM d, yyyy");

            ViewBag.DateEnrolled = dateEnrolledFormatted;

            AccountUpdateRequest accountUpdateRequest = new AccountUpdateRequest
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                BankName = account.BankName,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                BankBranch = account.BankBranch,
                InitialBalance = account.InitialBalance,
                Visibility = account.Visibility
            };

            return PartialView("~/Views/Shared/AccountForms/_UpdateAccountPartialView.cshtml", accountUpdateRequest);
        }

    }
}
