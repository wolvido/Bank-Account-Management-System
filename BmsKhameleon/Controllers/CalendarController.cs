using BmsKhameleon.Core.DTO.AccountDTOs;
using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BmsKhameleon.UI.Controllers
{
    [Authorize]
    public class CalendarController(ITransactionsService transactionsService, IAccountsService accountsService) : Controller
    {
        private readonly ITransactionsService _transactionsService = transactionsService;
        private readonly IAccountsService _accountsService = accountsService;

        [HttpGet]
        [Route("[action]/{accountId}/{date?}")]
        public async Task<IActionResult> Calendar(Guid accountId, DateTime? date)
        {
            date ??= DateTime.Now;

            AccountResponse? accountExists = await _accountsService.GetAccountById(accountId);

            if(accountExists == null)
            {
                return BadRequest("Account does not exist");
            }

            var accountDateEnrolled = accountExists.DateEnrolled ?? throw new InvalidOperationException("Account has no enrolled date");

            var accountMonthEnrolled = accountDateEnrolled.Month;
            var accountYearEnrolled = accountDateEnrolled.Year;

            ViewBag.accountMonthEnrolled = accountMonthEnrolled;
            ViewBag.accountYearEnrolled = accountYearEnrolled;

            List<DailyTransactionsAggregateResponse> monthlyTransactionsAggregate = await _transactionsService.GetMonthlyTransactionsAggregate(date.Value, accountId);

            return View(monthlyTransactionsAggregate);
        }
    }
}
