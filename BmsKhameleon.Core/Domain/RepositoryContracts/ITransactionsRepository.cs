using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Domain.Entities;

namespace BmsKhameleon.Core.Domain.RepositoryContracts
{
    public interface ITransactionsRepository
    {
        Task<bool> UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(Guid transactionId);
        Task<bool> CreateTransaction(Transaction transaction);
        Task<Transaction?> GetTransaction(Guid transactionId);
        Task<List<Transaction>> GetAllTransactionsForMonth(DateTime date, Guid accountId);
        Task<List<Transaction>> GetDepositsForDay(DateTime date, Guid accountId);
        Task<List<Transaction>> GetWithdrawalsForDay(DateTime date, Guid accountId);
        Task<List<Transaction>> GetTransactionsForAccount(Guid accountId);

    }
}
