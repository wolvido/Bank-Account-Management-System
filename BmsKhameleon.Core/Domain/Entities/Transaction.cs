using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.Domain.Entities
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }

        [Required(ErrorMessage = "Account ID is required.")]
        public Guid AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; } = null!; //navigation property

        [Required(ErrorMessage = "Transaction date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Transaction date must be a valid date.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [DataType(DataType.Currency, ErrorMessage = "must be a valid currency.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        [StringLength(10, ErrorMessage = "Transaction type cannot exceed 10 characters.")]
        public required string TransactionType { get; set; }

        [StringLength(10, ErrorMessage = "Transaction medium cannot exceed 10 characters.")]
        [Required(ErrorMessage = "Transaction medium is required.")]
        public required string TransactionMedium { get; set; }

        [StringLength(70, ErrorMessage = "Note cannot exceed 70 characters.")]
        public string? Note { get; set; }

        //cash transaction properties
        [StringLength(20, ErrorMessage = "Cash transaction type cannot exceed 20 characters.")]
        public string? CashTransactionType { get; set; }

        //cheque transaction properties
        [StringLength(50, ErrorMessage = "Payee name cannot exceed 70 characters.")]
        public string? Payee { get; set; }
        [StringLength(25, ErrorMessage = "Cheque bank name cannot exceed 25 characters.")]
        public string? ChequeBankName { get; set; }
        [StringLength(15, ErrorMessage = "Cheque number cannot exceed 15 characters.")]
        public string? ChequeNumber { get; set; }
    }
}
