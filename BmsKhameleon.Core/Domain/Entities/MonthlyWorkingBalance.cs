using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.Domain.Entities
{
    public class MonthlyWorkingBalance
    {
        public required Guid MonthlyWorkingBalanceId { get; set; }
        public required Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public required DateTime Date { get; set; }
        public required decimal WorkingBalance { get; set; }
    }
}
