using BmsKhameleon.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class CashTransactionResponse
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? Amount { get; set; }
        public string? TransactionType { get; set; }
        public string? Note { get; set; }

        //cash transaction properties
        public string? CashTransactionType { get; set; }
    }

    public static partial class TransactionExtensions
    {
        public static CashTransactionResponse ToTransactionCashResponse(this Transaction transaction)
        {
            return new CashTransactionResponse
            {
                TransactionId = transaction.TransactionId,
                AccountId = transaction.AccountId,
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                Note = transaction.Note,
                CashTransactionType = transaction.CashTransactionType
            };
        }
    }
}
