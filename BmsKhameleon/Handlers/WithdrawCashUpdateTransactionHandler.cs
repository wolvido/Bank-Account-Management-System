﻿using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.UI.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BmsKhameleon.UI.Handlers
{
    public class WithdrawCashUpdateTransactionHandler : IUpdateTransactionHandler
    {
        public bool CanHandle(string transactionType, string transactionMedium)
        {
            return transactionType == TransactionType.Withdraw.ToString() && transactionMedium == TransactionMedium.Cash.ToString();
        }

        public IActionResult HandleUpdateTransaction(TransactionResponse transactionResponse)
        {
            var transactionUpdateRequest = transactionResponse.ToCashTransactionUpdateRequest();

            return new PartialViewResult
            {
                ViewName = "~/Views/Shared/TransactionForms/_UpdateWithdrawCashTransactionPartial.cshtml",
                ViewData = new ViewDataDictionary<CashTransactionUpdateRequest>(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = transactionUpdateRequest
                }
            };

        }
    }
}
