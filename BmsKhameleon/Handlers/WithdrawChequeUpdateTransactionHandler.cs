using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.UI.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BmsKhameleon.UI.Handlers
{
    public class WithdrawChequeUpdateTransactionHandler : IUpdateTransactionHandler
    {
        public bool CanHandle(string transactionType, string transactionMedium)
        {
            return transactionType == TransactionType.Withdraw.ToString() && transactionMedium == TransactionMedium.Cheque.ToString();
        }

        public IActionResult HandleUpdateTransaction(TransactionResponse transactionResponse)
        {
            var chequeTransactionCreateRequest = new ChequeTransactionCreateRequest
            {
                AccountId = transactionResponse.AccountId,
                TransactionDate = transactionResponse.TransactionDate,
                Amount = transactionResponse.Amount,
                TransactionType = Enum.Parse<TransactionType>(transactionResponse.TransactionType ?? throw new InvalidOperationException("Invalid Transaction Type")),
                Note = transactionResponse.Note,
                Payee = transactionResponse.Payee ?? throw new InvalidOperationException("Payee empty"),
                ChequeBankName = transactionResponse.ChequeBankName ?? throw new InvalidOperationException("Cheque Bank Name empty"),
                ChequeNumber = transactionResponse.ChequeNumber ?? throw new InvalidOperationException("Cheque Number empty")
            };

            return new PartialViewResult
            {
                ViewName = "~/Views/Shared/TransactionForms/_WithdrawChequeTransactionPartial.cshtml",
                ViewData = new ViewDataDictionary<ChequeTransactionCreateRequest>(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = chequeTransactionCreateRequest
                }
            };
        }
    }
}
