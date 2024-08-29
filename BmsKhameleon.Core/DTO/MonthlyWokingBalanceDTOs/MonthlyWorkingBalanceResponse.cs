using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.DTO.MonthlyWokingBalanceDTOs
{
    public class MonthlyWorkingBalanceResponse
    {
        public required Guid MonthlyWorkingBalanceId { get; set; }
        public required Guid AccountId { get; set; }
        public required DateTime Date { get; set; }
        public required decimal WorkingBalance { get; set; }
    }

    public static class MonthlyWorkingBalanceExtensions
    {
        public static MonthlyWorkingBalanceResponse ToMonthlyWorkingBalanceResponse(this MonthlyWorkingBalance monthlyWorkingBalance)
        {
            return new MonthlyWorkingBalanceResponse
            {
                MonthlyWorkingBalanceId = monthlyWorkingBalance.MonthlyWorkingBalanceId,
                AccountId = monthlyWorkingBalance.AccountId,
                Date = monthlyWorkingBalance.Date,
                WorkingBalance = monthlyWorkingBalance.WorkingBalance
            };
        }
    }
}
