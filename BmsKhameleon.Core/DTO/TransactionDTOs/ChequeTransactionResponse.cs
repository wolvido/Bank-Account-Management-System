using BmsKhameleon.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;


namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class ChequeTransactionResponse
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionType { get; set; }
        public required string TransactionMedium { get; set; }
        public string? Note { get; set; }

        //cheque transaction properties
        public string? Payee { get; set; }
        public string? ChequeBankName { get; set; }
        public string? ChequeNumber { get; set; }
    }

    public partial class TransactionExtensions
    {
        public static ChequeTransactionResponse ToTransactionChequeResponse(this Transaction transaction)
        {
            return new ChequeTransactionResponse
            {
                TransactionId = transaction.TransactionId,
                AccountId = transaction.AccountId,
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                TransactionMedium = "Cheque",
                Note = transaction.Note,
                Payee = transaction.Payee,
                ChequeBankName = transaction.ChequeBankName,
                ChequeNumber = transaction.ChequeNumber
            };
        }
    }
}
