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

            if (account == null)
            {
                return new List<DailyTransactionsAggregateResponse>();
            }

            var monthTransactions = await _transactionsRepository.GetAllTransactionsForMonth(date, accountId);

            List<DailyTransactionsAggregateResponse> monthlyTransactionsAggregate = new List<DailyTransactionsAggregateResponse>();

            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                DateTime currentDate = new DateTime(date.Year, date.Month, i);

                var deposits = await GetDepositsForDay(currentDate, accountId);
                var withdrawals = await GetWithdrawalsForDay(currentDate, accountId);
                
                decimal totalDeposits = deposits.Sum(transaction => transaction.Amount);
                decimal totalWithdrawals = withdrawals.Sum(transaction => transaction.Amount);
                decimal totalBalance = account.InitialBalance;

                if (true/*totalDeposits > 0 || totalWithdrawals > 0*/)
                {
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
                else if(monthlyTransactionsAggregate.Count > 0)
                {
                    //add the same total balance as the last previous added
                    monthlyTransactionsAggregate.Add(new DailyTransactionsAggregateResponse
                    {
                        AccountId = accountId,
                        Date = currentDate,
                        TotalBalance = monthlyTransactionsAggregate.Last().TotalBalance,
                        TotalWithdrawal = 0
                    });
                }
            }
            
            return monthlyTransactionsAggregate;
        }
    }
}
