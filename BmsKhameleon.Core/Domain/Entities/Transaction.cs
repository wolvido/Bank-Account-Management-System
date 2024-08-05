using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.Domain.Entities
{
    public class Transaction
    {
        Guid TransactionId { get; set; }
        Guid AccountId { get; set; }
        Account Account { get; set; } = null!; //navigation property
        DateTime? TransactionDate { get; set; }
        int? TransactionAmount { get; set; }
        string? TransactionType { get; set; }
        string? TransactionMedium { get; set; }
        string? Note { get; set; }

        //cash transaction properties
        string? CashTransactionType { get; set; }

        //cheque transaction properties
        string? Payee { get; set; }
        string? ChequeBankName { get; set; }
        int? ChequeNumber { get; set; }
    }
}
