﻿using BmsKhameleon.Core.Domain.RepositoryContracts;
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
        /// <summary>
        /// get the last monthly balance before or equal the given date
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="date"></param>
        /// <returns>the last monthly balance from before or equal the given date</returns>
        public async Task<MonthlyWorkingBalance?> GetLastMonthlyBalance(Guid accountId, DateTime date)
        {
            MonthlyWorkingBalance? monthlyWorkingBalance = await _db.MonthlyWorkingBalances.Where(m => 
                m.AccountId == accountId && 
                m.Date <= date)
                .OrderByDescending(m => m.Date)
                .FirstOrDefaultAsync();

            return monthlyWorkingBalance ?? null;
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
            var monthlyWorkingBalance = await _db.MonthlyWorkingBalances.FindAsync(existingMonthBalance.MonthlyWorkingBalanceId);

            if (monthlyWorkingBalance == null)
            {
                throw new NotImplementedException("Monthly balance does not exist");
            }

            monthlyWorkingBalance.WorkingBalance += amount;
            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> InitialBalanceMonthAdjustment(Guid accountId, decimal amountToRemove, decimal amountToAdd)
        {
            //for all monthly balances of the account, adjust the working balance
            List<MonthlyWorkingBalance> monthlyBalances = await _db.MonthlyWorkingBalances.Where(m => m.AccountId == accountId).ToListAsync();

            if(monthlyBalances.Count == 0)
            {
                throw new NotImplementedException("No monthly balances found for the account");
            }

            foreach (MonthlyWorkingBalance monthlyBalance in monthlyBalances)
            {
                monthlyBalance.WorkingBalance -= amountToRemove;
                monthlyBalance.WorkingBalance += amountToAdd;
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> RemoveFromMonthlyBalance(MonthlyWorkingBalance existingMonthBalance, decimal amountToRemove)
        {
            var monthlyWorkingBalance = await _db.MonthlyWorkingBalances.FindAsync(existingMonthBalance.MonthlyWorkingBalanceId);

            if (monthlyWorkingBalance == null)
            {
                throw new NotImplementedException("Monthly balance does not exist");
            }

            monthlyWorkingBalance.WorkingBalance -= amountToRemove;
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

        public async Task<bool> AddToSucceedingMonthlyBalances(Guid accountId, DateTime date, decimal amount)
        {
            //grab all the monthly balances after the given date
            List<MonthlyWorkingBalance> succeedingMonthlyBalances = await _db.MonthlyWorkingBalances.Where(
                m => m.AccountId == accountId && 
                m.Date > date)
                    .ToListAsync();

            foreach (MonthlyWorkingBalance monthlyBalance in succeedingMonthlyBalances)
            {
                monthlyBalance.WorkingBalance += amount;
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> RemoveFromSucceedingMonthlyBalances(Guid accountId, DateTime date, decimal amount)
        {
            List<MonthlyWorkingBalance> succeedingMonthlyBalances = await _db.MonthlyWorkingBalances.Where(
                    m => m.AccountId == accountId && 
                         m.Date > date)
                .ToListAsync();

            foreach (MonthlyWorkingBalance monthlyBalance in succeedingMonthlyBalances)
            {
                monthlyBalance.WorkingBalance -= amount;
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
