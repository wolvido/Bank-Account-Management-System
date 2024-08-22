using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Domain.RepositoryContracts;
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

            var existingMonthlyBalance = await GetMonthlyBalance(accountId, date);
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
            else if (transactionType == "withdrawal")
            {
                transactionAmount = transaction.Amount * -1;
            }
            else
            {
                throw new ArgumentException("Invalid transaction type.");
            }

            var monthDate = new DateTime(transaction.TransactionDate.Year, transaction.TransactionDate.Month, 1);
            var existingMonthlyBalance = await GetMonthlyBalance(transaction.AccountId, monthDate);
            if (existingMonthlyBalance == null)
            {
                var createResult = await CreateMonthlyBalance(transaction.AccountId, monthDate, transactionAmount);
                return createResult;
            }

            var result = await _monthlyBalanceRepository.AddToMonthlyBalance(existingMonthlyBalance, transactionAmount);
            return result;
        }

        public async Task<bool> EditTransactionFromMonth(Guid accountId, DateTime date, decimal formerTransactionAmount,
            decimal newTransactionAmount)
        {
            throw new NotImplementedException();
        }

        public async Task<MonthlyWorkingBalance?> GetMonthlyBalance(Guid accountId, DateTime date)
        {
            var monthlyBalance = await _monthlyBalanceRepository.GetMonthlyBalance(accountId, date);
            if (monthlyBalance == null)
            {
                return null;
            }
            return monthlyBalance;
        }
    }
}
