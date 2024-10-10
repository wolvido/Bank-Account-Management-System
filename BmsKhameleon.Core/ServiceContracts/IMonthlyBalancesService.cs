using BmsKhameleon.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.DTO.MonthlyWokingBalanceDTOs;

namespace BmsKhameleon.Core.ServiceContracts
{
    public interface IMonthlyBalancesService
    {
        public Task<bool> AddTransactionToMonth(Transaction transaction);

        public Task<bool> RemoveTransactionFromMonth(Transaction transaction);
        public Task<MonthlyWorkingBalanceResponse?> GetMonthlyBalance(Guid accountId, DateTime date);

        public Task<MonthlyWorkingBalanceResponse?> GetLastMonthlyBalance(Guid accountId, DateTime date);

        public Task<bool> InitialBalanceMonthAdjustment(Guid accountId, decimal amountToRemove, decimal amountToAdd);

        public Task<bool> DeleteMonthBalance(Guid accountId, DateTime date);
        public Task<List<MonthlyWorkingBalanceResponse>> GetAllMonthlyBalances(Guid accountId);
    }
}
