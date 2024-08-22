using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.Domain.RepositoryContracts
{
    public interface IMonthlyBalancesRepository
    {
        public Task<MonthlyWorkingBalance?> GetMonthlyBalance(Guid accountId, DateTime date);

        public Task<bool> CreateMonthlyBalance(MonthlyWorkingBalance monthlyWorkingBalance);

        public Task<bool> AddToMonthlyBalance(MonthlyWorkingBalance monthlyWorkingBalance, decimal amount);

        public Task<bool> InitialBalanceMonthAdjustment(Guid accountId, decimal formerInitialBalance, decimal newInitialBalance);
    }
}
