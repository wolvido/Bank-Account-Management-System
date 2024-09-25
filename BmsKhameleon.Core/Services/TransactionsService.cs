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
            
            //check if account exists
            var account = await _accountsRepository.GetAccount(existingTransaction.AccountId);
            if(account == null)
            {
                throw new ArgumentException("Account does not exist");
            }

            //update monthly balances
            var removeFromMonthBalance =  await _monthlyBalances.RemoveTransactionFromMonth(existingTransaction);
            var addToMonthBalance = await _monthlyBalances.AddTransactionToMonth(transactionUpdateRequest.ToTransaction());
            if(removeFromMonthBalance == false || addToMonthBalance == false)
            {
                throw new ArgumentException("Failed to update monthly balances");
            }

            //update working balance
            if (existingTransaction.TransactionType == TransactionType.Deposit.ToString() || existingTransaction.TransactionType == TransactionType.Deposit.ToString().ToLower())
            {
                var workingBalanceWithdrawResult = await _accountsRepository.WithdrawFromWorkingBalance(existingTransaction.AccountId, existingTransaction.Amount);
                if (workingBalanceWithdrawResult == false)
                {
                    throw new ArgumentException("Failed to withdraw transaction from working balance");
                }
            }
            else
            {
                var workingBalanceDepositResult = await _accountsRepository.DepositToWorkingBalance(existingTransaction.AccountId, existingTransaction.Amount);
                if (workingBalanceDepositResult == false)
                {
                    throw new ArgumentException("Failed to deposit transaction to working balance");
                }
            }

            if (transactionUpdateRequest.TransactionType == TransactionType.Deposit)
            {
                var workingBalanceDepositResult = await _accountsRepository.DepositToWorkingBalance(transactionUpdateRequest.AccountId, transactionUpdateRequest.Amount);
                if (workingBalanceDepositResult == false)
                {
                    throw new ArgumentException("Failed to deposit transaction to working balance");
                }
            }
            else
            {
                var workingBalanceWithdrawResult = await _accountsRepository.WithdrawFromWorkingBalance(transactionUpdateRequest.AccountId, transactionUpdateRequest.Amount);
                if (workingBalanceWithdrawResult == false)
                {
                    throw new ArgumentException("Failed to withdraw transaction from working balance");
                }
            }

            bool result = await _transactionsRepository.UpdateTransaction(transactionUpdateRequest.ToTransaction());
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

            //update monthly balances
            bool removeFromMonthBalance = await _monthlyBalances.RemoveTransactionFromMonth(transaction);
            if(removeFromMonthBalance == false)
            {
                throw new ArgumentException("Failed to update monthly balances");
            }

            //update working balance
            if (transaction.TransactionType == TransactionType.Deposit.ToString() || transaction.TransactionType == TransactionType.Deposit.ToString().ToLower())
            {
                var workingBalanceWithdrawResult = await _accountsRepository.WithdrawFromWorkingBalance(transaction.AccountId, transaction.Amount);
                if (workingBalanceWithdrawResult == false)
                {
                    throw new ArgumentException("Failed to withdraw transaction from working balance");
                }
            }
            else
            {
                var workingBalanceDepositResult = await _accountsRepository.DepositToWorkingBalance(transaction.AccountId, transaction.Amount);
                if (workingBalanceDepositResult == false)
                {
                    throw new ArgumentException("Failed to deposit transaction to working balance");
                }
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
                //set bank name for cheque transactions
                var bankName = account.BankName;
                chequeTransactionCreateRequest.ChequeBankName = bankName;

                //withdraw transaction from working balance
                var workingBalanceWithdrawResult = await _accountsRepository.WithdrawFromWorkingBalance(chequeTransactionCreateRequest.AccountId, chequeTransactionCreateRequest.Amount);
                if(workingBalanceWithdrawResult == false)
                {
                    throw new ArgumentException("Failed to withdraw cheque transaction from working balance");
                }

            }
            else
            {
                //deposit transaction to working balance
                var workingBalanceDepositResult = await _accountsRepository.DepositToWorkingBalance(chequeTransactionCreateRequest.AccountId, chequeTransactionCreateRequest.Amount);
                if(workingBalanceDepositResult == false)
                {
                    throw new ArgumentException("Failed to deposit cheque transaction to working balance");
                }
            }

            //create transaction
            bool result = await _transactionsRepository.CreateTransaction(chequeTransactionCreateRequest.ToTransaction());

            //update monthly balances
            bool monthlyBalanceUpdateResult = await _monthlyBalances.AddTransactionToMonth(chequeTransactionCreateRequest.ToTransaction());
            if(monthlyBalanceUpdateResult == false)
            {

               throw new ArgumentException("Failed to update monthly balances");
            }

            return result;
        }

        public async Task<bool> CreateCashTransaction(CashTransactionCreateRequest cashTransactionCreateRequest)
        {
            var transactionType = cashTransactionCreateRequest.TransactionType.ToString().ToLower();

            if(transactionType is "withdrawal" or "withdraw")
            {
                //withdraw transaction from working balance
                var workingBalanceWithdrawResult = await _accountsRepository.WithdrawFromWorkingBalance(cashTransactionCreateRequest.AccountId, cashTransactionCreateRequest.Amount);
                if(workingBalanceWithdrawResult == false)
                {
                    throw new ArgumentException("Failed to withdraw cash transaction from working balance");
                }
            }
            else
            {
                //deposit transaction to working balance
                var workingBalanceDepositResult = await _accountsRepository.DepositToWorkingBalance(cashTransactionCreateRequest.AccountId, cashTransactionCreateRequest.Amount);
                if(workingBalanceDepositResult == false)
                {
                    throw new ArgumentException("Failed to deposit cash transaction to working balance");
                }
            }

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
        ///     aggregates all transactions of a given day then aggregates them into the given
        /// </summary>
        /// <param name="date"></param>
        /// <param name="accountId"></param>
        /// <returns> returns a list of daily transactions aggregates for the given month </returns>
        public async Task<List<DailyTransactionsAggregateResponse>> GetMonthlyTransactionsAggregate(DateTime date, Guid accountId)
        {
            var account = await _accountsRepository.GetAccount(accountId);
            if (account == null)
            {
                return new List<DailyTransactionsAggregateResponse>();
            }

            //var monthTransactions = await _transactionsRepository.GetAllTransactionsForMonth(date, accountId);

            List<DailyTransactionsAggregateResponse> monthlyTransactionsAggregate = new List<DailyTransactionsAggregateResponse>();

            //populate each day with aggregated transactions of the day
            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                DateTime currentDate = new DateTime(date.Year, date.Month, i);

                //sum all deposits and withdrawals for the day
                var depositsForDay = await GetDepositsForDay(currentDate, accountId);
                decimal totalDepositsForDay = depositsForDay.Sum(transaction => transaction.Amount);
                var withdrawalsForDay = await GetWithdrawalsForDay(currentDate, accountId);
                decimal totalWithdrawalsForDay = withdrawalsForDay.Sum(transaction => transaction.Amount);

                //add previous day total balance to the current day total balance
                var lastAggregatedDay = monthlyTransactionsAggregate.LastOrDefault();
                decimal totalBalance = lastAggregatedDay?.TotalBalance ?? 0;

                //aggregate the day and inject to the month
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

            //for each item in monthlyTransactionsAggregate, add in the total balance from the previousMonthBalance
            DateTime previousMonth = new DateTime(date.AddMonths(-1).Year, date.AddMonths(-1).Month, 1);
            MonthlyWorkingBalanceResponse? previousMonthWorkingBalance = await _monthlyBalances.GetLastMonthlyBalance(accountId, previousMonth);
            decimal previousMonthBalance = previousMonthWorkingBalance?.WorkingBalance ?? account.InitialBalance;
            foreach (var dailyAggregate in monthlyTransactionsAggregate)
            {
                dailyAggregate.TotalBalance += previousMonthBalance;
            }

            return monthlyTransactionsAggregate;
        }

        public async Task<DailyTransactionsAggregateResponse> GetDailyTransactionsAggregate(DateTime date, Guid accountId)
        {
            var account = await _accountsRepository.GetAccount(accountId);
            if(account == null)
            {
                throw new Exception("Account does not exist");
            }

            var lastMonthWorkingBalance = await _monthlyBalances.GetLastMonthlyBalance(accountId, date.AddMonths(-1));
            var lastMonthBalance = lastMonthWorkingBalance?.WorkingBalance ?? account.InitialBalance;

            //get every transaction from the first day of this month to the given date
            var transactions = await _transactionsRepository.GetAllTransactionsForMonth(date, accountId);
            var transactionsUpToDate = transactions.Where(transaction => transaction.TransactionDate <= date).ToList();

            //aggregate all deposits and withdrawals
            var deposits = transactionsUpToDate.Where(transaction => transaction.TransactionType == TransactionType.Deposit.ToString()).ToList();
            var withdrawals = transactionsUpToDate.Where(transaction => transaction.TransactionType == TransactionType.Withdraw.ToString()).ToList();
            
            //sum all deposits and withdrawals
            var totalDepositAmount = deposits.Sum(transaction => transaction.Amount);
            var totalWithdrawalAmount = withdrawals.Sum(transaction => transaction.Amount);

            //calculate total balance
            var totalBalance = lastMonthBalance + totalDepositAmount - totalWithdrawalAmount;

            return new DailyTransactionsAggregateResponse
            {
                AccountId = accountId,
                Date = date,
                TotalBalance = totalBalance,
                TotalWithdrawal = totalWithdrawalAmount
            };

        }

        public async Task<List<TransactionResponse>> GetTransactionsForAccount(Guid accountId)
        {
            List<Transaction> transactions = await _transactionsRepository.GetTransactionsForAccount(accountId);
            return transactions.Select(transaction => transaction.ToTransactionResponse()).ToList();
        }

        public Task<List<TransactionResponse>> SortTransactions(List<TransactionResponse> transactions, string sortBy, SortOrderOptions sortOrder)
        {

            if (sortOrder == SortOrderOptions.Ascending)
            {
                switch (sortBy)
                {
                    case nameof(TransactionResponse.TransactionMedium):
                        transactions = transactions.OrderBy(transaction => transaction.TransactionMedium).ToList();
                        break;
                    case nameof(TransactionResponse.Payee):
                        transactions = transactions.OrderBy(transaction => transaction.Payee).ToList();
                        break;
                    case nameof(TransactionResponse.Amount):
                        transactions = transactions.OrderBy(transaction => transaction.Amount).ToList();
                        break;
                    case nameof(TransactionResponse.Note):
                        transactions = transactions.OrderBy(transaction => transaction.Note).ToList();
                        break;
                }
            }
            else
            {
                switch (sortBy)
                {
                    case nameof(TransactionResponse.TransactionMedium):
                        transactions = transactions.OrderByDescending(transaction => transaction.TransactionMedium).ToList();
                        break;
                    case nameof(TransactionResponse.Payee):
                        transactions = transactions.OrderByDescending(transaction => transaction.Payee).ToList();
                        break;
                    case nameof(TransactionResponse.Amount):
                        transactions = transactions.OrderByDescending(transaction => transaction.Amount).ToList();
                        break;
                    case nameof(TransactionResponse.Note):
                        transactions = transactions.OrderByDescending(transaction => transaction.Note).ToList();
                        break;
                }
            }

            return Task.FromResult(transactions);
            
        }
    }
}
