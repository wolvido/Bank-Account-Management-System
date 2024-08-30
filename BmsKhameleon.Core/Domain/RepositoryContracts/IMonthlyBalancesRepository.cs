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

        public Task<MonthlyWorkingBalance?> GetPreviousMonthlyBalance(Guid accountId, DateTime date);

        public Task<bool> CreateMonthlyBalance(MonthlyWorkingBalance monthlyWorkingBalance);

        public Task<bool> AddToMonthlyBalance(MonthlyWorkingBalance existingMonthBalance, decimal amount);

        public Task<bool> InitialBalanceMonthAdjustment(Guid accountId, decimal amountToRemove, decimal amountToAdd);

        public Task<bool> RemoveFromMonthlyBalance(MonthlyWorkingBalance existingMonthBalance, decimal amountToRemove);

        public Task<bool> DeleteMonthBalance(Guid accountId, DateTime date);
        public Task<List<MonthlyWorkingBalance>> GetAllMonthlyBalances(Guid accountId);
    }
}
