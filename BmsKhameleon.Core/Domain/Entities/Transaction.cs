using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.Domain.Entities
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        Account Account { get; set; } = null!; //navigation property
        public DateTime? TransactionDate { get; set; }
        public int? Amount { get; set; }
        public string? TransactionType { get; set; }
        public string? TransactionMedium { get; set; }
        public string? Note { get; set; }

        //cash transaction properties
        public string? CashTransactionType { get; set; }

        //cheque transaction properties
        public string? Payee { get; set; }
        public string? ChequeBankName { get; set; }
        public int? ChequeNumber { get; set; }
    }
}
