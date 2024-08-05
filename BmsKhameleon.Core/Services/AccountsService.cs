using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;
using BmsKhameleon.Core.ServiceContracts;

namespace BmsKhameleon.Core.Services
{
    public class AccountsService : IAccountsService
    {
        private Guid _selectedAccountId;
        public void SetSelectedAccount(Guid accountId)
        {
            _selectedAccountId = accountId;
        }
    }
}
