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
    public class AccountsRepository(AccountDbContext db) : IAccountsRepository
    {
        private readonly AccountDbContext _db = db;

        public async Task<bool> CreateAccount(Account account)
        {
            account.WorkingBalance = account.InitialBalance;
            _db.Accounts.Add(account);
            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Account?> GetAccount(Guid accountId)
        {
            Account? account = await _db.Accounts.FindAsync(accountId);
            return account;
        }

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _db.Accounts.ToListAsync();
        }

        public async Task<bool> UpdateAccount(Account account)
        {
            Account? existingAccount = await _db.Accounts.FindAsync(account.AccountId);

            if (existingAccount == null)
            {
                return false;
            };

            existingAccount.AccountName = account.AccountName;
            existingAccount.BankName = account.BankName;
            existingAccount.AccountNumber = account.AccountNumber;
            existingAccount.AccountType = account.AccountType;
            existingAccount.BankBranch = account.BankBranch;
            existingAccount.WorkingBalance = (existingAccount.WorkingBalance - existingAccount.InitialBalance) + account.InitialBalance;
            existingAccount.InitialBalance = account.InitialBalance;
            existingAccount.Visibility = account.Visibility;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccount(Guid accountId)
        {
            Account? account = await _db.Accounts.FindAsync(accountId);

            if(account == null)
            {
                return false;
            }

            _db.Accounts.Remove(account);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<List<Account>> GetAccountsByBank(string bankName)
        {
            return await _db.Accounts.Where(account => account.BankName == bankName).ToListAsync();
        }

        public async Task<List<Account>> GetAccountsByName(string accountName)
        {
            return await _db.Accounts.Where(account => account.AccountName == accountName).ToListAsync();
        }

        public async Task<List<Account>> GetAccountsByNumber(string accountNumber)
        {
            return await _db.Accounts.Where(account => account.AccountNumber.Equals(accountNumber)).ToListAsync();
        }

        public async Task<List<Account>> GetAccountsByBankAndName(string bankName, string accountName)
        {
            return await _db.Accounts.Where(account => account.BankName == bankName && account.AccountName == accountName).ToListAsync();
        }

        public Task<List<Account>> GetAccountsByBankAndNumber(string bankName, string accountNumber)
        {
            return _db.Accounts.Where(account => account.BankName == bankName && account.AccountNumber.Equals(accountNumber)).ToListAsync();
        }

        public Task<List<string>> GetAllAccountBanks()
        {
            return _db.Accounts.Select(account => account.BankName).Distinct().ToListAsync();
        }

        public async Task<bool> DepositToWorkingBalance(Guid accountId, decimal amount)
        {
            var account = await _db.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return false;
            }
            account.WorkingBalance += amount;
            return await _db.SaveChangesAsync() > 0;

        }

        public async Task<bool> WithdrawFromWorkingBalance(Guid accountId, decimal amount)
        {
            var account = await _db.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return false;
            }
            account.WorkingBalance -= amount;
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
