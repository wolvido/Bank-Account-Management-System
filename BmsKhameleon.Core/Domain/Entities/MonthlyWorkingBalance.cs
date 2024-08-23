using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.Domain.Entities
{
    public class MonthlyWorkingBalance
    {
        [Key]
        public required Guid MonthlyWorkingBalanceId { get; set; }
        public required Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public required DateTime Date { get; set; }
        [DataType(DataType.Currency, ErrorMessage = "Working balance must be a valid currency.")]
        public required decimal WorkingBalance { get; set; }
    }
}
