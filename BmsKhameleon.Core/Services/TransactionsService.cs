using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Domain.RepositoryContracts;
using BmsKhameleon.Core.DTO.TransactionDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.Core.ServiceContracts;

namespace BmsKhameleon.Core.Services
{
    public class TransactionsService(ITransactionsRepository transactionsRepository, IAccountsRepository accountsRepository) : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository = transactionsRepository;
        private readonly IAccountsRepository _accountsRepository = accountsRepository;
        public async Task<bool> UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest)
        {
            bool result = await _transactionsRepository.UpdateTransaction(transactionUpdateRequest.ToTransaction());
            return result;
        }

        public async Task<bool> DeleteTransaction(Guid transactionId)
        {
            bool result = await _transactionsRepository.DeleteTransaction(transactionId);
            return result;
        }

        public async Task<bool> CreateChequeTransaction(ChequeTransactionCreateRequest chequeTransactionCreateRequest)
        {
            bool result = await _transactionsRepository.CreateTransaction(chequeTransactionCreateRequest.ToTransaction());
            return result;
        }

        public async Task<bool> CreateCashTransaction(CashTransactionCreateRequest cashTransactionCreateRequest)
        {
            bool result = await _transactionsRepository.CreateTransaction(cashTransactionCreateRequest.ToTransaction());
            return result;
        }

        public async Task<TransactionResponse> GetTransaction(Guid transactionId)
        {
            Transaction transaction = await _transactionsRepository.GetTransaction(transactionId);
            return transaction.ToTransactionResponse();
        }

        public async Task<List<TransactionResponse>> GetDepositsForDay(DateTime date, Guid accountId)
        {
            List<Transaction> transactions = await _transactionsRepository.GetDepositsForDay(date, accountId);
            return transactions.Select(transaction => transaction.ToTransactionResponse()).ToList();
        }

        public async Task<List<TransactionResponse>> GetWithdrawalsForDay(DateTime date, Guid accountId)
        {
            List<Transaction> transactions = await _transactionsRepository.GetWithdrawalsForDay(date, accountId);
            return transactions.Select(transaction => transaction.ToTransactionResponse()).ToList();
        }

        public async Task<List<TransactionResponse>> GetCashWithdrawalsForDay(DateTime date, Guid accountId)
        {
            var withdrawals = await GetWithdrawalsForDay(date, accountId);
            var cashWithdrawals = withdrawals.Where(transaction => transaction.TransactionMedium == TransactionMedium.Cash.ToString()).ToList();
            return cashWithdrawals;
        }

        public async Task<List<TransactionResponse>> GetChequeWithdrawalsForDay(DateTime date, Guid accountId)
        {
            var withdrawals = await GetWithdrawalsForDay(date, accountId);
            var chequeWithdrawals = withdrawals.Where(transaction => transaction.TransactionMedium == TransactionMedium.Cheque.ToString()).ToList();
            return chequeWithdrawals;
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

            var monthTransactions = await _transactionsRepository.GetAllTransactionsForMonth(date, accountId);

            List<DailyTransactionsAggregateResponse> monthlyTransactionsAggregate = new List<DailyTransactionsAggregateResponse>();

            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                DateTime currentDate = new DateTime(date.Year, date.Month, i);

                var deposits = await GetDepositsForDay(currentDate, accountId);
                var withdrawals = await GetWithdrawalsForDay(currentDate, accountId);
                
                int? totalDeposits = deposits.Sum(transaction => transaction.Amount);
                int? totalWithdrawals = withdrawals.Sum(transaction => transaction.Amount);
                int? totalBalance = account.InitialBalance;

                totalBalance += totalDeposits;
                totalBalance -= totalWithdrawals;

                monthlyTransactionsAggregate.Add(new DailyTransactionsAggregateResponse
                {
                    AccountId = accountId,
                    Date = currentDate,
                    TotalBalance = totalBalance,
                    TotalWithdrawal = totalWithdrawals
                });
            }
            
            return monthlyTransactionsAggregate;
        }
    }
}
