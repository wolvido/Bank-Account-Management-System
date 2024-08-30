using BmsKhameleon.Core.DTO.AccountDTOs;
using BmsKhameleon.Core.DTO.TransactionDTOs;

namespace BmsKhameleon.UI.ViewModels
{
    public class TransactionOverviewViewModel
    {
        public required AccountResponse AccountResponse { get; set; }
        public required DailyTransactionsAggregateResponse DailyTransactionsAggregateResponse { get; set; }
    }
}
