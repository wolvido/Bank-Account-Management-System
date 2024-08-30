using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.UI.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BmsKhameleon.UI.Handlers
{
    public class DepositChequeUpdateTransactionHandler : IUpdateTransactionHandler
    {
        public bool CanHandle(string transactionType, string transactionMedium)
        {
            return transactionType == TransactionType.Deposit.ToString() && transactionMedium == TransactionMedium.Cheque.ToString();
        }

        public IActionResult HandleUpdateTransaction(TransactionResponse transactionResponse)
        {
            var chequeTransactionCreateRequest = new ChequeTransactionCreateRequest
            {
                AccountId = transactionResponse.AccountId,
                TransactionDate = transactionResponse.TransactionDate,
                Amount = transactionResponse.Amount,
                TransactionType = Enum.Parse<TransactionType>(transactionResponse.TransactionType ?? throw new InvalidOperationException("Transaction Type invalid")),
                Note = transactionResponse.Note,
                Payee = transactionResponse.Payee ?? throw new InvalidOperationException("Payee empty"),
                ChequeBankName = transactionResponse.ChequeBankName ?? throw new InvalidOperationException("Cheque Bank Name empty"),
                ChequeNumber = transactionResponse.ChequeNumber ?? throw new InvalidOperationException("Cheque Number empty")
            };

            return new PartialViewResult
            {
                ViewName = "~/Views/Shared/TransactionForms/_DepositChequeTransactionPartial.cshtml",
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
