using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Enums;

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string? TransactionType { get; set; }
        public string? TransactionMedium { get; set; }
        public string? Note { get; set; }

        //cash transaction properties
        public string? CashTransactionType { get; set; }

        //cheque transaction properties
        public string? Payee { get; set; }
        public string? ChequeBankName { get; set; }
        public string? ChequeNumber { get; set; }


        public CashTransactionUpdateRequest ToCashTransactionUpdateRequest()
        {
            return new CashTransactionUpdateRequest
            {
                TransactionId = TransactionId,
                AccountId = AccountId,
                TransactionDate = TransactionDate,
                Amount = Amount,
                TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), TransactionType ?? throw new InvalidOperationException($"Invalid Transaction type '{TransactionType}'"), true ),
                Note = Note,
                CashTransactionType = CashTransactionType
            };
        }

        public ChequeTransactionUpdateRequest ToChequeTransactionUpdateRequest()
        {
            return new ChequeTransactionUpdateRequest
            {
                TransactionId = TransactionId,
                AccountId = AccountId,
                TransactionDate = TransactionDate,
                Amount = Amount,
                TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), TransactionType ?? throw new InvalidOperationException($"Invalid Transaction type '{TransactionType}'"), true ),
                Note = Note,
                Payee = Payee,
                ChequeBankName = ChequeBankName,
                ChequeNumber = ChequeNumber
            };
        }

    }

    public static partial class TransactionExtensions
    {
       public static TransactionResponse ToTransactionResponse(this Transaction transaction)
       {
            return new TransactionResponse
            {
                TransactionId = transaction.TransactionId,
                AccountId = transaction.AccountId,
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                TransactionType = transaction.TransactionType,
                TransactionMedium = transaction.TransactionMedium,
                Note = transaction.Note,
                CashTransactionType = transaction.CashTransactionType,
                Payee = transaction.Payee,
                ChequeBankName = transaction.ChequeBankName,
                ChequeNumber = transaction.ChequeNumber
            };
       }
    }
}
