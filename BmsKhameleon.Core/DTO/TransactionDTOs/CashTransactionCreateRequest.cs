using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Transaction date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Transaction date must be a valid date.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [DataType(DataType.Currency, ErrorMessage = "must be a valid currency.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        //[StringLength(10, ErrorMessage = "Transaction type cannot exceed 10 characters.")]
        public TransactionType TransactionType { get; set; }

        [StringLength(70, ErrorMessage = "Note cannot exceed 70 characters.")]
        public string? Note { get; set; }

        //cash transaction properties
        //[StringLength(20, ErrorMessage = "Cash transaction type cannot exceed 20 characters.")]
        [Required(ErrorMessage = "Cash transaction type is required.")]
        public required string CashTransactionType { get; set; }

        public Transaction ToTransaction()
        { 
            return new Transaction
            {
                TransactionId = Guid.NewGuid(),
                AccountId = AccountId,
                TransactionDate = TransactionDate,
                Amount = Amount,
                TransactionType = TransactionType.ToString(),
                TransactionMedium = "Cash",
                Note = Note,
                CashTransactionType = CashTransactionType
            };
        }
    }
}
