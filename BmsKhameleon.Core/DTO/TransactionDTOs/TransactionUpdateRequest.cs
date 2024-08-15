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
    public class TransactionUpdateRequest
    {
        public Guid TransactionId { get; set; }

        [Required(ErrorMessage = "Account ID is required.")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Transaction date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Transaction date must be a valid date.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [DataType(DataType.Currency, ErrorMessage = "must be a valid currency.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        [StringLength(10, ErrorMessage = "Transaction type cannot exceed 10 characters.")]
        public TransactionType TransactionType { get; set; }

        [StringLength(10, ErrorMessage = "Transaction medium cannot exceed 10 characters.")]
        [Required(ErrorMessage = "Transaction medium is required.")]
        public TransactionMedium TransactionMedium { get; set; }

        [StringLength(70, ErrorMessage = "Note cannot exceed 70 characters.")]
        public string? Note { get; set; }

        //cash transaction properties
        [StringLength(15, ErrorMessage = "Cash transaction type cannot exceed 15 characters.")]
        public string? CashTransactionType { get; set; }

        //cheque transaction properties
        [StringLength(50, ErrorMessage = "Payee name cannot exceed 70 characters.")]
        public string? Payee { get; set; }
        [StringLength(25, ErrorMessage = "Cheque bank name cannot exceed 25 characters.")]
        public string? ChequeBankName { get; set; }
        [StringLength(15, ErrorMessage = "Cheque number cannot exceed 15 characters.")]
        public string? ChequeNumber { get; set; }

        public Transaction ToTransaction()
        {

           return new Transaction
            {
                TransactionId = TransactionId,
                AccountId = AccountId,
                TransactionDate = TransactionDate,
                Amount = Amount,
                TransactionType = TransactionType.ToString(),
                TransactionMedium = TransactionMedium.ToString(),
                Note = Note,
                CashTransactionType = CashTransactionType,
                Payee = Payee,
                ChequeBankName = ChequeBankName,
                ChequeNumber = ChequeNumber
            };
        }

    }
}
