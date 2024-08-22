using BmsKhameleon.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.ServiceContracts
{
    public interface IMonthlyBalancesService
    {
        public Task<bool> AddTransactionToMonth(Transaction transaction);

        public Task<bool> EditTransactionFromMonth(Guid accountId, DateTime date, decimal formerTransactionAmount, decimal newTransactionAmount);

        public Task<MonthlyWorkingBalance?> GetMonthlyBalance(Guid accountId, DateTime date);
    }
}
