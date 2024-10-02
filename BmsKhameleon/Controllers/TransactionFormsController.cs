using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.Core.ServiceContracts;
using BmsKhameleon.UI.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BmsKhameleon.UI.Controllers
{
    [Authorize]
    public class TransactionFormsController(ITransactionsService transactionsService) : Controller
    {
        private readonly ITransactionsService _transactionsService = transactionsService;

        //Create Deposit Cash
        [HttpGet]
        [Route("[action]/{accountId}/{date}")]
        public IActionResult CreateDepositCashPartial(Guid accountId, DateTime date)
        {
            ViewBag.AccountId = accountId;
            ViewBag.Date = date;
            return PartialView("~/Views/Shared/TransactionForms/_DepositCashTransactionPartial.cshtml");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateDepositCash(CashTransactionCreateRequest? depositCashTransactionRequest)
        {
            if (depositCashTransactionRequest == null)
            {
                return BadRequest("Invalid deposit cash transaction request.");
            }

            bool result = await _transactionsService.CreateCashTransaction(depositCashTransactionRequest);

            if (result == false)
            {
                return BadRequest("Unable to create the deposit cash transaction. Please try again.");
            }

            var formattedDate = depositCashTransactionRequest.TransactionDate.ToString("yyyy-MM-dd");

            return RedirectToAction("TransactionsOverview", "TransactionsOverview",
                new {
                    accountId = depositCashTransactionRequest.AccountId,
                    date = formattedDate
                });
        }

        //Create Deposit Cheque
        [HttpGet]
        [Route("[action]/{accountId}/{date}")]
        public IActionResult CreateDepositChequePartial(Guid accountId, DateTime date)
        {
            ViewBag.AccountId = accountId;
            ViewBag.Date = date;
            return PartialView("~/Views/Shared/TransactionForms/_DepositChequeTransactionPartial.cshtml");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateDepositCheque(ChequeTransactionCreateRequest? chequeTransactionCreateRequest)
        {
            if (chequeTransactionCreateRequest == null)
            {
                return BadRequest("Invalid deposit cheque transaction request.");
            }

            bool result = await _transactionsService.CreateChequeTransaction(chequeTransactionCreateRequest);

            if (result == false)
            {
                return BadRequest("Unable to create the deposit cheque transaction. Please try again.");
            }

            var formattedDate = chequeTransactionCreateRequest.TransactionDate.ToString("yyyy-MM-dd");

            return RedirectToAction("TransactionsOverview", "TransactionsOverview",
                new
                {
                    accountId = chequeTransactionCreateRequest.AccountId,
                    date = formattedDate
                });
        }

        //Create Withdraw Cash
        [HttpGet]
        [Route("[action]/{accountId}/{date}")]
        public IActionResult CreateWithdrawCashPartial(Guid accountId, DateTime date)
        {
            ViewBag.AccountId = accountId;
            ViewBag.Date = date;
            return PartialView("~/Views/Shared/TransactionForms/_WithdrawCashTransactionPartial.cshtml");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateWithdrawCash(CashTransactionCreateRequest? withdrawCashTransactionRequest)
        {
            if (withdrawCashTransactionRequest == null)
            {
                return BadRequest("Invalid deposit cash transaction request.");
            }

            bool result = await _transactionsService.CreateCashTransaction(withdrawCashTransactionRequest);

            if (result == false)
            {
                return BadRequest("Unable to create the withdraw cash transaction. Please try again.");
            }

            var formattedDate = withdrawCashTransactionRequest.TransactionDate.ToString("yyyy-MM-dd");

            return RedirectToAction("TransactionsOverview", "TransactionsOverview",
                new {
                    accountId = withdrawCashTransactionRequest.AccountId,
                    date = formattedDate
                });
        }

        //Create Withdraw Cheque
        [HttpGet]
        [Route("[action]/{accountId}/{date}")]
        public IActionResult CreateWithdrawChequePartial(Guid accountId, DateTime date)
        {
            ViewBag.AccountId = accountId;
            ViewBag.Date = date;
            return PartialView("~/Views/Shared/TransactionForms/_WithdrawChequeTransactionPartial.cshtml");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateWithdrawCheque(ChequeTransactionCreateRequest? withdrawChequeTransactionRequest)
        {

            if (withdrawChequeTransactionRequest == null)
            {
                return BadRequest("Invalid deposit cheque transaction request.");
            }

            bool result = await _transactionsService.CreateChequeTransaction(withdrawChequeTransactionRequest);

            if (result == false)
            {
                return BadRequest("Unable to create the withdraw cheque transaction. Please try again.");
            }

            var formattedDate = withdrawChequeTransactionRequest.TransactionDate.ToString("yyyy-MM-dd");

            return RedirectToAction("TransactionsOverview", "TransactionsOverview",
                new
                {
                    accountId = withdrawChequeTransactionRequest.AccountId,
                    date = formattedDate
                });
        }

        [HttpGet]
        [Route("[action]/{transactionId}/{transactionType}/{transactionMedium}")]
        public async Task<IActionResult> UpdateTransactionPartial(Guid transactionId, string transactionType, string transactionMedium, [FromServices] UpdateTransactionHandlerFactory updateTransactionHandlerFactory)
        {
            var transaction = await _transactionsService.GetTransaction(transactionId);
            if(transaction == null)
            {
                return BadRequest("Transaction not found.");
            }

            var handler = updateTransactionHandlerFactory.GetHandler(transactionType, transactionMedium);
            return handler.HandleUpdateTransaction(transaction);

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateTransactionCash(CashTransactionUpdateRequest? cashTransactionUpdateRequest)
        {
            if (cashTransactionUpdateRequest == null)
            {
                return BadRequest("Invalid transaction update request.");
            }

            var transactionUpdateRequest = cashTransactionUpdateRequest.ToTransactionUpdateRequest();

            bool result = await _transactionsService.UpdateTransaction(transactionUpdateRequest);

            if (result == false)
            {
                return BadRequest("Unable to update the transaction. Please try again.");
            }

            var formattedDate = transactionUpdateRequest.TransactionDate.ToString("yyyy-MM-dd");

            return RedirectToAction("TransactionsOverview", "TransactionsOverview",
                new
                {
                    accountId = transactionUpdateRequest.AccountId,
                    date = formattedDate
                });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateTransactionCheque(ChequeTransactionUpdateRequest? chequeTransactionUpdateRequest)
        {
            if (chequeTransactionUpdateRequest == null)
            {
                return BadRequest("Invalid transaction update request.");
            }

            var transactionUpdateRequest = chequeTransactionUpdateRequest.ToTransactionUpdateRequest();

            bool result = await _transactionsService.UpdateTransaction(transactionUpdateRequest);

            if (result == false)
            {
                return BadRequest("Unable to update the transaction. Please try again.");
            }

            var formattedDate = transactionUpdateRequest.TransactionDate.ToString("yyyy-MM-dd");

            return RedirectToAction("TransactionsOverview", "TransactionsOverview",
                new
                {
                    accountId = transactionUpdateRequest.AccountId,
                    date = formattedDate
                });
        }

        [HttpPost]
        [Route("[action]/{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(Guid transactionId)
        {
            var transaction = await _transactionsService.GetTransaction(transactionId);
            if (transaction == null)
            {
                return BadRequest("Transaction not found.");
            }

            var result = await _transactionsService.DeleteTransaction(transactionId);
            if (result == false) 
            {
                return BadRequest("Unable to delete the transaction. Please try again.");
            }

            return RedirectToAction("TransactionsOverview", "TransactionsOverview",
                new
                {
                    accountId = transaction.AccountId,
                    date = transaction.TransactionDate.ToString("yyyy-MM-dd")
                });
        }
    }
}
