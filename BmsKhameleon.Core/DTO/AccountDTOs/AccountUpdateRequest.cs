using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.DTO.AccountDTOs
{
    public class AccountUpdateRequest
    {
        Guid AccountId { get; set; }
        string? AccountName { get; set; }
        string? BankName { get; set; }
        int? AccountNumber { get; set; }
        string? AccountType { get; set; }
        string? BankBranch { get; set; }
        int? InitialBalance { get; set; }
        int? WorkingBalance { get; set; }
        DateTime? DateEnrolled { get; set; }
        bool Visibility { get; set; }
    }
}
