using BmsKhameleon.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.DTO.AccountDTOs;
using BmsKhameleon.Core.Enums;

namespace BmsKhameleon.Core.ServiceContracts
{
    public interface IAccountsService
    {
        public Task<bool> CreateAccount(AccountCreateRequest accountCreateRequest);

        public Task<AccountResponse?> GetAccountById(Guid accountId);

        public Task<List<AccountResponse>> GetAllAccounts();

        public Task<bool> UpdateAccount(AccountUpdateRequest accountUpdateRequest);

        public Task<bool> DeleteAccount(Guid accountId);

        public Task<List<AccountResponse>> GetFilteredAccounts(string searchQuery, string? bankName);

        public Task<List<AccountResponse>> SortAccounts(List<AccountResponse> accounts, string sortBy, SortOrderOptions sortOrder);

        public Task<List<string>> GetAllAccountBanks();
    }
}
