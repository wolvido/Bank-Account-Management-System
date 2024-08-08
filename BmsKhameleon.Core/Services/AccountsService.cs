using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.Domain.RepositoryContracts;
using BmsKhameleon.Core.DTO.AccountDTOs;
using BmsKhameleon.Core.Enums;
using BmsKhameleon.Core.ServiceContracts;

namespace BmsKhameleon.Core.Services
{
    public class AccountsService(IAccountsRepository accountsRepository) : IAccountsService
    {
        private readonly IAccountsRepository _accountsRepository = accountsRepository;

        public async Task<bool> CreateAccount(AccountCreateRequest accountCreateRequest)
        {
            bool result = await _accountsRepository.CreateAccount(accountCreateRequest.ToAccount());
            return result;
        }

        public async Task<List<AccountResponse>> GetAllAccounts()
        {
            List<Account> accounts = await _accountsRepository.GetAllAccounts();
            return accounts.Select(account => account.ToAccountResponse()).ToList();
        }

        public async Task<bool> UpdateAccount(AccountUpdateRequest accountUpdateRequest)
        {
            bool result = await _accountsRepository.UpdateAccount(accountUpdateRequest.ToAccount());
            return result;
        }

        public async Task<bool> DeleteAccount(Guid accountId)
        {
            bool result = await _accountsRepository.DeleteAccount(accountId);
            return result;
        }

        private async Task<List<Account>> GetAccountsBySearchQuery(string searchQuery)
        {
            if (int.TryParse(searchQuery, out int accountNumber))
            {
                return await _accountsRepository.GetAccountsByNumber(accountNumber);
            }

            return await _accountsRepository.GetAccountsByName(searchQuery);
        }

        private async Task<List<Account>> GetAccountsByBankAndSearchQuery(string bankName, string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return await _accountsRepository.GetAccountsByBank(bankName);
            }

            if (int.TryParse(searchQuery, out int accountNumber))
            {
                return await _accountsRepository.GetAccountsByBankAndNumber(bankName, accountNumber);
            }

            return await _accountsRepository.GetAccountsByBankAndName(bankName, searchQuery);
        }

        /// <summary>
        ///     retrieve a list of accounts filtered by search query and or bank
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="bankName"></param>
        /// <returns>Accounts Filtered by search query and or bank</returns>
        public async Task<List<AccountResponse>> GetFilteredAccounts(string searchQuery, string? bankName)
        {

            List<Account> accounts;

            if (bankName != null)
            {
                accounts = await GetAccountsByBankAndSearchQuery(bankName, searchQuery);
            }
            else
            {
                accounts = await GetAccountsBySearchQuery(searchQuery);
            }

            return accounts.Select(account => account.ToAccountResponse()).ToList();
        }

        public Task<List<AccountResponse>> SortAccounts(List<AccountResponse> accounts, string sortBy, SortOrderOptions sortOrder)
        {

            switch (sortBy)
            {
                case "AccountName":
                    if(sortOrder == SortOrderOptions.Ascending)
                    {
                        return Task.FromResult(accounts.OrderBy(account => account.AccountName).ToList());
                    }

                    return Task.FromResult(accounts.OrderByDescending(account => account.AccountName).ToList());

                case "AccountNumber":
                    if(sortOrder == SortOrderOptions.Ascending)
                    {
                        return Task.FromResult(accounts.OrderBy(account => account.AccountNumber).ToList());
                    }

                    return Task.FromResult(accounts.OrderByDescending(account => account.AccountNumber).ToList());

                case "BankName":
                    if(sortOrder == SortOrderOptions.Ascending)
                    {
                        return Task.FromResult(accounts.OrderBy(account => account.BankName).ToList());
                    }

                    return Task.FromResult(accounts.OrderByDescending(account => account.BankName).ToList());

                case "Balance":
                    if(sortOrder == SortOrderOptions.Ascending)
                    {
                        return Task.FromResult(accounts.OrderBy(account => account.WorkingBalance).ToList());
                    }

                    return Task.FromResult(accounts.OrderByDescending(account => account.WorkingBalance).ToList());

                default:
                    throw new ArgumentException("Invalid sortBy parameter");
            }
        }

        /// <summary>
        ///     retrieve a list of all unique bank names registered to accounts
        /// </summary>
        /// <returns>bank names</returns>
        public Task<List<string>> GetAllAccountBanks()
        {
            return _accountsRepository.GetAllAccountBanks();
        }
    }
}
