using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.DTO.AccountDTOs
{
    public class AccountResponse
    {
        public Guid AccountId { get; set; }
        public required string AccountName { get; set; }
        public required string BankName { get; set; }
        public required string AccountNumber { get; set; }
        public required string AccountType { get; set; }
        public string? BankBranch { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal? WorkingBalance { get; set; }
        public DateTime? DateEnrolled { get; set; }
        public bool Visibility { get; set; }

        public Guid? ApplicationUserId  { get; set; }
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
                Visibility = account.Visibility,
                ApplicationUserId = account.ApplicationUserId
            };
        }
    }
}
