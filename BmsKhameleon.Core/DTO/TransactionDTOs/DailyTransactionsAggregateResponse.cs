using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class DailyTransactionsAggregateResponse
    {
        Guid AccountId { get; set; }
        DateTime Date { get; set; }
        int? Balance { get; set; }
        int? Withdrawal { get; set; }
    }
}
