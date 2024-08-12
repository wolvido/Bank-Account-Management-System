using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Enums;

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class CashTransactionCreateRequest
    {
        public Guid AccountId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionType? TransactionType { get; set; }
        public string? Note { get; set; }

        //cash transaction properties
        public string? CashTransactionType { get; set; }

        public Transaction ToTransaction()
        { 
            return new Transaction
            {
                AccountId = AccountId,
                TransactionDate = TransactionDate,
                Amount = Amount,
                TransactionType = TransactionType.ToString(),
                Note = Note,
                CashTransactionType = CashTransactionType
            };
        }
    }
}
