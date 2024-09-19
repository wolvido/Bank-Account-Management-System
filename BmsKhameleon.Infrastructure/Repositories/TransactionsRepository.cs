using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Domain.RepositoryContracts;
using BmsKhameleon.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BmsKhameleon.Infrastructure.Repositories
{
    public class TransactionsRepository(AccountDbContext db): ITransactionsRepository
    {
        private readonly AccountDbContext _db = db;

        public async Task<bool> UpdateTransaction(Transaction transaction)
        {
            Transaction? existingTransaction = await _db.Transactions.FindAsync(transaction.TransactionId);
            if (existingTransaction == null)
            {
                return false;
            }

            existingTransaction.TransactionDate = transaction.TransactionDate;
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.TransactionType = transaction.TransactionType;
            existingTransaction.TransactionMedium = transaction.TransactionMedium;
            existingTransaction.Note = transaction.Note;
            existingTransaction.CashTransactionType = transaction.CashTransactionType;
            existingTransaction.Payee = transaction.Payee;
            existingTransaction.ChequeBankName = transaction.ChequeBankName;
            existingTransaction.ChequeNumber = transaction.ChequeNumber;

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTransaction(Guid transactionId)
        {
            Transaction? transaction = await _db.Transactions.FindAsync(transactionId);

            if (transaction == null)
            {
                return false;
            }

            _db.Transactions.Remove(transaction);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateTransaction(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Transaction?> GetTransaction(Guid transactionId)
        {
            return await _db.Transactions.FindAsync(transactionId);
        }

        public async Task<List<Transaction>> GetAllTransactionsForMonth(DateTime date, Guid accountId)
        {
            return await _db.Transactions.Where(transaction => 
                transaction.TransactionDate.Month == date.Month && 
                transaction.TransactionDate.Year == date.Year && 
                transaction.AccountId == accountId)
                    .ToListAsync();
        }

        public async Task<List<Transaction>> GetDepositsForDay(DateTime date, Guid accountId)
        {
            return await _db.Transactions.Where(transaction => 
                transaction.TransactionDate == date && 
                transaction.TransactionType == "Deposit" && 
                transaction.AccountId == accountId)
                    .ToListAsync();
        }

        public async Task<List<Transaction>> GetWithdrawalsForDay(DateTime date, Guid accountId)
        {
            return await _db.Transactions.Where(transaction => 
                transaction.TransactionDate == date && 
                (transaction.TransactionType == "Withdrawal" || transaction.TransactionType == "Withdraw") && 
                transaction.AccountId == accountId)
                    .ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsForAccount(Guid accountId)
        {
            var transactions = await _db.Transactions.Where(transaction => transaction.AccountId == accountId).ToListAsync();
            return transactions;
        }

    }
}
