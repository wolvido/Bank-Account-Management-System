using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.DTO.AccountDTOs
{
    public class AccountResponse
    {
        public Guid AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public int? AccountNumber { get; set; }
        public string? AccountType { get; set; }
        public string? BankBranch { get; set; }
        public int? InitialBalance { get; set; }
        public int? WorkingBalance { get; set; }
        public DateTime? DateEnrolled { get; set; }
        public bool Visibility { get; set; }
    }

    public static class AccountExtensions
    {
        public static AccountResponse ToAccountResponse(this Account account)
        {
            return new AccountResponse
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                BankName = account.BankName,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                BankBranch = account.BankBranch,
                InitialBalance = account.InitialBalance,
                WorkingBalance = account.WorkingBalance,
                DateEnrolled = account.DateEnrolled,
                Visibility = account.Visibility
            };
        }
    }
}
