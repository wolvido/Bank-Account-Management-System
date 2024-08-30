using BmsKhameleon.Core.DTO.TransactionDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BmsKhameleon.UI.Factories
{
    public interface IUpdateTransactionHandler
    {
        bool CanHandle(string transactionType, string transactionMedium);
        IActionResult HandleUpdateTransaction(TransactionResponse transactionResponse);
    }
}
