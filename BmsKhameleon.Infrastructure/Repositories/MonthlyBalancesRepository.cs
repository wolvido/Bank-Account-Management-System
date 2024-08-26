using BmsKhameleon.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BmsKhameleon.Infrastructure.Repositories
{
    public class MonthlyBalancesRepository(AccountDbContext db) : IMonthlyBalancesRepository
    {
        private readonly AccountDbContext _db = db;
        public async Task<MonthlyWorkingBalance?> GetMonthlyBalance(Guid accountId, DateTime date)
        {
            MonthlyWorkingBalance? monthlyWorkingBalance = await _db.MonthlyWorkingBalances.Where(m => 
                m.AccountId == accountId && 
                m.Date.Month == date.Month && 
                m.Date.Year == date.Year)
                .FirstOrDefaultAsync();

            return monthlyWorkingBalance;
        }

        public async Task<bool> CreateMonthlyBalance(MonthlyWorkingBalance monthlyWorkingBalance)
        {
            _db.MonthlyWorkingBalances.Add(monthlyWorkingBalance);
            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        /// <summary>
        /// add an amount to an existing monthly balance
        /// </summary>
        /// <param name="existingMonthBalance"></param>
        /// <param name="amount"></param>
        /// <returns>true if the task is successful, false if not</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AddToMonthlyBalance(MonthlyWorkingBalance existingMonthBalance, decimal amount)
        {
            existingMonthBalance.WorkingBalance += amount;
            _db.Entry(existingMonthBalance).State = EntityState.Modified;
            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> InitialBalanceMonthAdjustment(Guid accountId, decimal amountToRemove, decimal amountToAdd)
        {
            //for all monthly balances of the account, adjust the working balance
            List<MonthlyWorkingBalance> monthlyBalances = await _db.MonthlyWorkingBalances.Where(m => m.AccountId == accountId).ToListAsync();
            foreach (MonthlyWorkingBalance monthlyBalance in monthlyBalances)
            {
                monthlyBalance.WorkingBalance -= amountToRemove;
                monthlyBalance.WorkingBalance += amountToAdd;
                _db.Entry(monthlyBalance).State = EntityState.Modified;
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> RemoveFromMonthlyBalance(MonthlyWorkingBalance existingMonthBalance, decimal amountToRemove)
        {
            existingMonthBalance.WorkingBalance -= amountToRemove;
            _db.Entry(existingMonthBalance).State = EntityState.Modified;
            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> DeleteMonthBalance(Guid accountId, DateTime date)
        {
            _db.MonthlyWorkingBalances.RemoveRange(_db.MonthlyWorkingBalances.Where(m => m.AccountId == accountId && m.Date.Month == date.Month && m.Date.Year == date.Year));
            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<MonthlyWorkingBalance>> GetAllMonthlyBalances(Guid accountId)
        {
            var monthlyBalances = await _db.MonthlyWorkingBalances.Where(m => m.AccountId == accountId).ToListAsync();
            return monthlyBalances;
        }
    }
}
