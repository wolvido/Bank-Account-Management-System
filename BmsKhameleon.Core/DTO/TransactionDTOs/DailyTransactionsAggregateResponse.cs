

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class DailyTransactionsAggregateResponse
    {
        public Guid AccountId { get; set; }
        public DateTime Date { get; set; }
        public decimal? TotalBalance { get; set; }
        public decimal? TotalWithdrawal { get; set; }
    }
}
