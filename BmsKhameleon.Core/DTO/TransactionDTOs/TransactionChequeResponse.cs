using BmsKhameleon.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class TransactionChequeResponse
    {
        Guid TransactionId { get; set; }
        Guid AccountId { get; set; }
        DateTime? TransactionDate { get; set; }
        int? TransactionAmount { get; set; }
        TransactionType? TransactionType { get; set; }
        string? Note { get; set; }

        //cheque transaction properties
        public string? Payee { get; set; }
        public string? ChequeBankName { get; set; }
        public int? ChequeNumber { get; set; }
    }
}
