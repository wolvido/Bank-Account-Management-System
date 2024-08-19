using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.Domain.RepositoryContracts
{
    public interface IAccountsRepository
    {
        public Task<bool> CreateAccount(Account account);
        public Task<Account?> GetAccount(Guid accountId);
        public Task<List<Account>> GetAllAccounts();
        public Task<bool> UpdateAccount(Account account);
        public Task<bool> DeleteAccount(Guid accountId);

        public Task<List<Account>> GetAccountsByBank(string bankName);
        public Task<List<Account>> GetAccountsByName(string accountName);
        public Task<List<Account>> GetAccountsByNumber(string accountNumber);
        public Task<List<Account>> GetAccountsByBankAndName(string bankName, string accountName);
        public Task<List<Account>> GetAccountsByBankAndNumber(string bankName, string accountNumber);
        public Task<List<string>> GetAllAccountBanks();

    }
}
