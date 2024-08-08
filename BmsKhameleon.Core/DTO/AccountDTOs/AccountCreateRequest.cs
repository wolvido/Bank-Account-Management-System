

using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.DTO.AccountDTOs
{
    public class AccountCreateRequest
    {
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public int? AccountNumber { get; set; }
        public string? AccountType { get; set; }
        public string? BankBranch { get; set; }
        public int? InitialBalance { get; set; }
        public DateTime? DateEnrolled { get; set; }
        public bool Visibility { get; set; }

        public Account ToAccount()
        {
            return new Account
            {
                AccountName = AccountName,
                BankName = BankName,
                AccountNumber = AccountNumber,
                AccountType = AccountType,
                BankBranch = BankBranch,
                InitialBalance = InitialBalance,
                DateEnrolled = DateEnrolled,
                Visibility = Visibility
            };
        }
    }
}
