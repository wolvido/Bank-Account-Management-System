using BmsKhameleon.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmsKhameleon.Core.ServiceContracts
{
    public interface IAccountsService
    {
        public void SetSelectedAccount(Guid accountId);
    }
}
