using System.Globalization;
using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BmsKhameleon.UI.Controllers
{
    public class TransactionsOverviewController(ITransactionsService transactionsService) : Controller
    {
        private readonly ITransactionsService _transactionsService = transactionsService;

        [Route("[action]/{accountId}/{date}")]
        public IActionResult TransactionsOverview(Guid accountId, DateTime date)
        {
            ViewBag.AccountId = accountId;
            ViewBag.Date = date;
            return View();
        }

        [Route("[action]/{accountId}/{date}")]
        public async Task<IActionResult> CashDepositsTable(Guid accountId, DateTime date)
        {
            var transactions = await _transactionsService.GetCashDepositsForDay(date, accountId);

            return PartialView("~/Views/Shared/Transactiontables/_CashDepositTransactionsTablePartial.cshtml", transactions);
        }

        [Route("[action]/{accountId}/{date}")]
        public async Task<IActionResult> ChequeDepositsTable(Guid accountId, DateTime date)
        {
            var transactions = await _transactionsService.GetChequeDepositsForDay(date, accountId);

            return PartialView("~/Views/Shared/Transactiontables/_ChequeDepositTransactionsTablePartial.cshtml", transactions);
        }

        [Route("[action]/{accountId}/{date}")]
        public async Task<IActionResult> WithdrawalsTable(Guid accountId, DateTime date)
        {
            var transactions = await _transactionsService.GetWithdrawalsForDay(date, accountId);

            return PartialView("~/Views/Shared/Transactiontables/_WithdrawalTransactionsTablePartial.cshtml", transactions);
        }
    }
}
