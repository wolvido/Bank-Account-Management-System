using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Domain.RepositoryContracts;
using BmsKhameleon.Core.DTO.MonthlyWokingBalanceDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.Core.ServiceContracts;

namespace BmsKhameleon.Core.Services
{
    public class MonthlyBalancesService(IMonthlyBalancesRepository monthlyBalanceRepository, IAccountsRepository accountsRepository) : IMonthlyBalancesService
    {
        private readonly IMonthlyBalancesRepository _monthlyBalanceRepository = monthlyBalanceRepository;
        private readonly IAccountsRepository _accountsRepository = accountsRepository;
        private async Task<bool> CreateMonthlyBalance(Guid accountId, DateTime date, decimal workingBalance)
        {
            Account? account = await _accountsRepository.GetAccount(accountId);

            if (account == null)
            {
                throw new InvalidOperationException("Account does not exist");
            }

            MonthlyWorkingBalanceResponse? existingMonthlyBalance = await GetMonthlyBalance(accountId, date);
            if (existingMonthlyBalance != null && 
                existingMonthlyBalance.Date.Month == date.Month && 
                existingMonthlyBalance.Date.Year == date.Year)
            {
                throw new InvalidOperationException("Monthly balance already exists for this month");
            }

            var monthlyWorkingBalance = new MonthlyWorkingBalance
                {
                    MonthlyWorkingBalanceId = Guid.NewGuid(),
                    AccountId = accountId,
                    Date = date,
                    WorkingBalance = workingBalance+account.InitialBalance
                };

            bool result = await _monthlyBalanceRepository.CreateMonthlyBalance(monthlyWorkingBalance);
            return result;
        }

        public async Task<bool> AddTransactionToMonth(Transaction transaction)
        {
            var transactionType = transaction.TransactionType.ToLower();
            decimal transactionAmount;
            if (transactionType == "deposit")
            {
                transactionAmount = transaction.Amount;
            }
            else if (transactionType is "withdrawal" or "withdraw")
            {
                transactionAmount = transaction.Amount * -1;
            }
            else
            {
                throw new ArgumentException("Invalid transaction type.");
            }

            var monthDate = new DateTime(transaction.TransactionDate.Year, transaction.TransactionDate.Month, 1);
            var existingMonthBalanceResponse = await GetMonthlyBalance(transaction.AccountId, monthDate);
            if (existingMonthBalanceResponse == null)
            {
                var createResult = await CreateMonthlyBalance(transaction.AccountId, monthDate, transactionAmount);
                return createResult;
            }

            var existingMonthBalance = new MonthlyWorkingBalance() { 
                MonthlyWorkingBalanceId = existingMonthBalanceResponse.MonthlyWorkingBalanceId,
                AccountId = existingMonthBalanceResponse.AccountId,
                Date = existingMonthBalanceResponse.Date, 
                WorkingBalance = existingMonthBalanceResponse.WorkingBalance
            };

            var result = await _monthlyBalanceRepository.AddToMonthlyBalance(existingMonthBalance, transactionAmount);
            return result;
        }

        public async Task<bool> RemoveTransactionFromMonth(Transaction transaction)
        {
            var date = new DateTime(transaction.TransactionDate.Year, transaction.TransactionDate.Month, 1);

            decimal amount;
            var transactionType = transaction.TransactionType.ToLower();
            if(transaction.TransactionType.ToLower() == "deposit")
            {
                amount = transaction.Amount;
            }
            else if(transactionType is "withdrawal" or "withdraw")
            {
                amount = transaction.Amount * -1;
            }
            else
            {
                throw new ArgumentException("Invalid transaction type.");
            }

            var existingMonthBalanceResponse = await GetMonthlyBalance(transaction.AccountId, date);
            if (existingMonthBalanceResponse == null)
            {
                throw new InvalidOperationException("Monthly balance does not exist for this month");
            }

            var existingMonthBalance = new MonthlyWorkingBalance()
            {
                MonthlyWorkingBalanceId = existingMonthBalanceResponse.MonthlyWorkingBalanceId,
                AccountId = existingMonthBalanceResponse.AccountId,
                Date = existingMonthBalanceResponse.Date,
                WorkingBalance = existingMonthBalanceResponse.WorkingBalance
            };

            var result = await _monthlyBalanceRepository.RemoveFromMonthlyBalance(existingMonthBalance, amount);
            return result;
        }

        public async Task<MonthlyWorkingBalanceResponse?> GetMonthlyBalance(Guid accountId, DateTime date)
        {
            if(date.Day != 1)
            {
                date = new DateTime(date.Year, date.Month, 1);
            }

            var monthlyBalance = await _monthlyBalanceRepository.GetMonthlyBalance(accountId, date);
            if (monthlyBalance == null)
            {
                return null;
            }
            return monthlyBalance.ToMonthlyWorkingBalanceResponse();
        }

        public async Task<MonthlyWorkingBalanceResponse?> GetPreviousMonthlyBalance(Guid accountId, DateTime date)
        {
            if(date.Day != 1)
            {
                date = new DateTime(date.Year, date.Month, 1);
            }

            var result = await _monthlyBalanceRepository.GetPreviousMonthlyBalance(accountId, date);
            if (result == null)
            {
                return null;
            }
            return  result.ToMonthlyWorkingBalanceResponse();
        }

        public async Task<bool> InitialBalanceMonthAdjustment(Guid accountId, decimal amountToRemove, decimal amountToAdd)
        {
            bool result = await _monthlyBalanceRepository.InitialBalanceMonthAdjustment(accountId, amountToRemove, amountToAdd);
            return result;
        }

        public async Task<bool> DeleteMonthBalance(Guid accountId, DateTime date)
        {
            var result = await _monthlyBalanceRepository.DeleteMonthBalance(accountId, date);
            if (result == false)
            {
                throw new InvalidOperationException("Monthly balance does not exist for this month");
            }
            return result;
        }

        public async Task<List<MonthlyWorkingBalanceResponse>> GetAllMonthlyBalances(Guid accountId)
        {
            var result = await _monthlyBalanceRepository.GetAllMonthlyBalances(accountId);

            if (result == null)
            {
                throw new InvalidOperationException("No monthly balances found for this account");
            }

            //convert every result of result into onthly working balance response
            var resultConverted =  result.Select(x => x.ToMonthlyWorkingBalanceResponse()).ToList();

            return resultConverted;
        }
    }
}
