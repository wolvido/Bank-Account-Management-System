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
            var transactionUpdateRequest = transactionResponse.ToChequeTransactionUpdateRequest();

            return new PartialViewResult
            {
                ViewName = "~/Views/Shared/TransactionForms/_UpdateDepositChequeTransactionPartial.cshtml",
                ViewData = new ViewDataDictionary<ChequeTransactionUpdateRequest>(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = transactionUpdateRequest
                }
            };
        }
    }
}
