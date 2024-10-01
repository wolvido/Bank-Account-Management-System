using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.IdentityEntities;

namespace BmsKhameleon.Core.Domain.Entities
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(50, ErrorMessage = "Account name cannot exceed 50 characters.")]
        public required string AccountName { get; set; }

        [Required(ErrorMessage = "Bank name is required.")]
        [StringLength(25, ErrorMessage = "Bank name cannot exceed 25 characters.")]
        public required string BankName { get; set; }

        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(15, ErrorMessage = "Account number cannot exceed 60 characters.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Account number must be numeric.")]
        public required string AccountNumber { get; set; }

        [Required(ErrorMessage = "Account type is required.")]
        [StringLength(20, ErrorMessage = "Account type cannot exceed 20 characters.")]
        public required string AccountType { get; set; }

        [StringLength(30, ErrorMessage = "Bank branch cannot exceed 30 characters.")]
        public string? BankBranch { get; set; }

        [Required(ErrorMessage = "Initial balance is required.")]
        [DataType(DataType.Currency, ErrorMessage = "Working balance must be a valid currency.")]
        public decimal InitialBalance { get; set; }

        [Required(ErrorMessage = "Working balance is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Working balance must be greater than zero.")]
        [DataType(DataType.Currency, ErrorMessage = "Working balance must be a valid currency.")]
        public decimal? WorkingBalance { get; set; }

        [Required(ErrorMessage = "Date enrolled is required.")]
        [DataType(DataType.Date, ErrorMessage = "Date enrolled must be a valid date.")]
        public DateTime DateEnrolled { get; set; }

        [Required(ErrorMessage = "Visibility is required.")]
        public bool Visibility { get; set; }
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();
        public ICollection<MonthlyWorkingBalance> MonthlyWorkingBalances { get; } = new List<MonthlyWorkingBalance>();

        //IdentityUser
        public Guid? ApplicationUserId  { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }


    }
}
