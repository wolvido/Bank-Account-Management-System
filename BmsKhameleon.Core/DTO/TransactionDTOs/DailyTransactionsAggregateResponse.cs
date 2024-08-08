

namespace BmsKhameleon.Core.DTO.TransactionDTOs
{
    public class DailyTransactionsAggregateResponse
    {
        public Guid AccountId { get; set; }
        public DateTime Date { get; set; }
        public int? TotalBalance { get; set; }
        public int? TotalWithdrawal { get; set; }
    }
}
