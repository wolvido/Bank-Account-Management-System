using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class MonthlyTransactionsAggregateResponse
    {
        DailyTransactionsAggregateResponse[]? DailyTransactions { get; set; }
    }
}
