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
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult CreateDepositCashTransactionPartial()
        {
            return PartialView("~/Views/Shared/TransactionForms/_DepositCashTransactionPartial.cshtml");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateDepositCashTransaction(CashTransactionCreateRequest? depositCashTransactionRequest)
        {
            if(depositCashTransactionRequest == null)
            {
                return BadRequest("Invalid deposit cash transaction request.");
            }

            bool result = await _transactionsService.CreateCashTransaction(depositCashTransactionRequest);

            if(result == false)
            {
                return BadRequest("Unable to create the deposit cash transaction. Please try again.");
            }

            return RedirectToAction("TransactionsOverview",
                new {
                    accountId = depositCashTransactionRequest.AccountId,
                    date = depositCashTransactionRequest.TransactionDate
                });

        }


    }
}
