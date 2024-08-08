using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.Domain.Entities
{
    public class Account
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
        ICollection<Transaction> Transactions { get; } = new List<Transaction>();


    }
}
