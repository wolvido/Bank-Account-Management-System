
using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.DTO.AccountDTOs
{
    public class AccountUpdateRequest
    {
        public Guid AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountType { get; set; }
        public string? BankBranch { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal? WorkingBalance { get; set; }
        public bool Visibility { get; set; }

        public Account ToAccount()
        {
            return new Account
            {
                AccountId = AccountId,
                AccountName = AccountName,
                BankName = BankName,
                AccountNumber = AccountNumber,
                AccountType = AccountType,
                BankBranch = BankBranch,
                InitialBalance = InitialBalance,
                WorkingBalance = WorkingBalance,
                Visibility = Visibility

            };
        }
    }
}
