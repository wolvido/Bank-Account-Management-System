

using System.ComponentModel.DataAnnotations;
using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.DTO.AccountDTOs
{
    public class AccountCreateRequest
    {
        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(50, ErrorMessage = "Account name cannot exceed 50 characters.")]
        public string? AccountName { get; set; }
        [Required(ErrorMessage = "Bank name is required.")]
        [StringLength(25, ErrorMessage = "Bank name cannot exceed 25 characters.")]
        public string? BankName { get; set; }
        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(60, ErrorMessage = "Account number cannot exceed 60 characters.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Account number must be numeric.")]
        public string? AccountNumber { get; set; }
        [Required(ErrorMessage = "Account type is required.")]
        [StringLength(20, ErrorMessage = "Account type cannot exceed 20 characters.")]
        public string? AccountType { get; set; }
        [StringLength(30, ErrorMessage = "Bank branch cannot exceed 30 characters.")]
        public string? BankBranch { get; set; }
        [Required(ErrorMessage = "Initial balance is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Initial balance must be greater than zero.")]
        public decimal InitialBalance { get; set; }
        [Required(ErrorMessage = "Date enrolled is required.")]
        [DataType(DataType.Date, ErrorMessage = "Date enrolled must be a valid date.")]
        public DateTime? DateEnrolled { get; set; }
        [Required(ErrorMessage = "Visibility is required.")]
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
