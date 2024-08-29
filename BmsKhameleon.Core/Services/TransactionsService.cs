using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Domain.RepositoryContracts;
using BmsKhameleon.Core.DTO.MonthlyWokingBalanceDTOs;
using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.Core.ServiceContracts;

namespace BmsKhameleon.Core.Services
{
    public class TransactionsService(ITransactionsRepository transactionsRepository, IAccountsRepository accountsRepository, IMonthlyBalancesService monthlyBalances) : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository = transactionsRepository;
        private readonly IAccountsRepository _accountsRepository = accountsRepository;
        private readonly IMonthlyBalancesService _monthlyBalances = monthlyBalances;
        public async Task<bool> UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest)
        {
            Transaction? existingTransaction = await _transactionsRepository.GetTransaction(transactionUpdateRequest.TransactionId);
            if (existingTransaction == null)
            {
                return false;
            }

            bool result = await _transactionsRepository.UpdateTransaction(transactionUpdateRequest.ToTransaction());

            var removeFromMonthBalance =  await _monthlyBalances.RemoveTransactionFromMonth(existingTransaction);
            var addToMonthBalance = await _monthlyBalances.AddTransactionToMonth(transactionUpdateRequest.ToTransaction());

            if(removeFromMonthBalance == false || addToMonthBalance == false)
            {
                throw new ArgumentException("Failed to update monthly balances");
            }

            return result;
        }

        public async Task<bool> DeleteTransaction(Guid transactionId)
        {
            var transaction = await _transactionsRepository.GetTransaction(transactionId);

            if(transaction == null)
            {
                throw new ArgumentException("Transaction does not exist");
            }

            bool result = await _transactionsRepository.DeleteTransaction(transactionId);

            bool removeFromMonthBalance = await _monthlyBalances.RemoveTransactionFromMonth(transaction);
            if(removeFromMonthBalance == false)
            {
                throw new ArgumentException("Failed to update monthly balances");
            }

            return result;
        }

        public async Task<bool> CreateChequeTransaction(ChequeTransactionCreateRequest chequeTransactionCreateRequest)
        {
            string transactionType = chequeTransactionCreateRequest.TransactionType.ToString().ToLower();

            if(transactionType is "withdrawal" or "withdraw")
            {
                var account = await _accountsRepository.GetAccount(chequeTransactionCreateRequest.AccountId);
                if (account == null)
                {
                    throw new ArgumentException("Account does not exist");
                }

                var bankName = account.BankName;
                chequeTransactionCreateRequest.ChequeBankName = bankName;
            }

            bool result = await _transactionsRepository.CreateTransaction(chequeTransactionCreateRequest.ToTransaction());

            bool monthlyBalanceUpdateResult = await _monthlyBalances.AddTransactionToMonth(chequeTransactionCreateRequest.ToTransaction());
            if(monthlyBalanceUpdateResult == false)
            {

               throw new ArgumentException("Failed to update monthly balances");
            }

            return result;
        }

        public async Task<bool> CreateCashTransaction(CashTransactionCreateRequest cashTransactionCreateRequest)
        {
            bool result = await _transactionsRepository.CreateTransaction(cashTransactionCreateRequest.ToTransaction());

            bool monthlyBalanceUpdateResult = await _monthlyBalances.AddTransactionToMonth(cashTransactionCreateRequest.ToTransaction());

            if(monthlyBalanceUpdateResult == false)
            {
                throw new ArgumentException("Failed to update monthly balances");
            }

            return result;
        }

        public async Task<TransactionResponse?> GetTransaction(Guid transactionId)
        {
            Transaction? transaction = await _transactionsRepository.GetTransaction(transactionId);
            if (transaction == null)
            {
                return null;
            }
            return transaction.ToTransactionResponse();
        }

        public async Task<List<TransactionResponse>> GetDepositsForDay(DateTime date, Guid accountId)
        {
            List<Transaction> transactions = await _transactionsRepository.GetDepositsForDay(date, accountId);
            return transactions.Select(transaction => transaction.ToTransactionResponse()).ToList();
        }

        public async Task<List<CashTransactionResponse>> GetCashDepositsForDay(DateTime date, Guid accountId)
        {
            var deposits = await GetDepositsForDay(date, accountId);
            var cashDeposits = deposits.Where(transaction => transaction.TransactionMedium == TransactionMedium.Cash.ToString()).ToList();
            var cashDepositsResponse = cashDeposits.Select(transaction => transaction.ToTransactionCashResponse()).ToList();
            return cashDepositsResponse;
        }

        public async Task<List<ChequeTransactionResponse>> GetChequeDepositsForDay(DateTime date, Guid accountId)
        {
            var deposits = await GetDepositsForDay(date, accountId);
            var chequeDeposits = deposits.Where(transaction => transaction.TransactionMedium == TransactionMedium.Cheque.ToString()).ToList();
            var chequeDepositsResponse = chequeDeposits.Select(transaction => transaction.ToTransactionChequeResponse()).ToList();
            return chequeDepositsResponse;
        }

        public async Task<List<TransactionResponse>> GetWithdrawalsForDay(DateTime date, Guid accountId)
        {
            List<Transaction> transactions = await _transactionsRepository.GetWithdrawalsForDay(date, accountId);
            return transactions.Select(transaction => transaction.ToTransactionResponse()).ToList();
        }

        public async Task<List<CashTransactionResponse>> GetCashWithdrawalsForDay(DateTime date, Guid accountId)
        {
            var withdrawals = await GetWithdrawalsForDay(date, accountId);
            var cashWithdrawals = withdrawals.Where(transaction => transaction.TransactionMedium == TransactionMedium.Cash.ToString()).ToList();
            var cashWithdrawalsResponse = cashWithdrawals.Select(transaction => transaction.ToTransactionCashResponse()).ToList();
            return cashWithdrawalsResponse;
        }

        public async Task<List<ChequeTransactionResponse>> GetChequeWithdrawalsForDay(DateTime date, Guid accountId)
        {
            var withdrawals = await GetWithdrawalsForDay(date, accountId);
            var chequeWithdrawals = withdrawals.Where(transaction => transaction.TransactionMedium == TransactionMedium.Cheque.ToString()).ToList();
            var chequeWithdrawalsResponse = chequeWithdrawals.Select(transaction => transaction.ToTransactionChequeResponse()).ToList();
            return chequeWithdrawalsResponse;
        }

        /// <summary>
        ///     aggregates all transactions of a given day and aggregates them into a given month
        /// </summary>
        /// <param name="date"></param>
        /// <param name="accountId"></param>
        /// <returns> returns a list of daily transactions aggregates for a given month </returns>
        public async Task<List<DailyTransactionsAggregateResponse>> GetMonthlyTransactionsAggregate(DateTime date, Guid accountId)
        {
            var account = await _accountsRepository.GetAccount(accountId);

            if (account == null)
            {
                return new List<DailyTransactionsAggregateResponse>();
            }

            var monthTransactions = await _transactionsRepository.GetAllTransactionsForMonth(date, accountId);

            List<DailyTransactionsAggregateResponse> monthlyTransactionsAggregate = new List<DailyTransactionsAggregateResponse>();

            //populate each day with aggregated transactions of the day
            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                DateTime currentDate = new DateTime(date.Year, date.Month, i);

                //grab all deposits and withdrawals for the day
                var depositsForDay = await GetDepositsForDay(currentDate, accountId);
                decimal totalDepositsForDay = depositsForDay.Sum(transaction => transaction.Amount);
                var withdrawalsForDay = await GetWithdrawalsForDay(currentDate, accountId);
                decimal totalWithdrawalsForDay = withdrawalsForDay.Sum(transaction => transaction.Amount);

                //add previous day total balance to the current day total balance
                var lastAggregatedDay = monthlyTransactionsAggregate.LastOrDefault();
                decimal totalBalance = lastAggregatedDay?.TotalBalance ?? 0;

                //aggregate and inject
                totalBalance += totalDepositsForDay;
                totalBalance -= totalWithdrawalsForDay;
                monthlyTransactionsAggregate.Add(new DailyTransactionsAggregateResponse
                {
                    AccountId = accountId,
                    Date = currentDate,
                    TotalBalance = totalBalance,
                    TotalWithdrawal = totalWithdrawalsForDay
                });

            }

            //for each item in monthlyTransactionsAggregate add in the total balance the previousMonthBalance
            DateTime previousMonth = new DateTime(date.AddMonths(-1).Year, date.AddMonths(-1).Month, 1);
            MonthlyWorkingBalanceResponse? previousMonthWorkingBalance = await _monthlyBalances.GetMonthlyBalance(accountId, previousMonth);
            decimal previousMonthBalance = previousMonthWorkingBalance?.WorkingBalance ?? account.InitialBalance;
            foreach (var dailyAggregate in monthlyTransactionsAggregate)
            {
                dailyAggregate.TotalBalance += previousMonthBalance;
            }

            return monthlyTransactionsAggregate;
        }

        public async Task<List<TransactionResponse>> GetTransactionsForAccount(Guid accountId)
        {
            List<Transaction> transactions = await _transactionsRepository.GetTransactionsForAccount(accountId);
            return transactions.Select(transaction => transaction.ToTransactionResponse()).ToList();
        }
    }
}
