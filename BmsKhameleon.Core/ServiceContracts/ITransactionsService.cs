using BmsKhameleon.Core.DTO.TransactionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BmsKhameleon.Core.Enums;

namespace BmsKhameleon.Core.ServiceContracts
{
    public interface ITransactionsService
    {
        Task<bool> UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest);
        Task<bool> DeleteTransaction(Guid transactionId);
        
        Task<bool> CreateChequeTransaction(ChequeTransactionCreateRequest chequeTransactionCreateRequest);
        Task<bool> CreateCashTransaction(CashTransactionCreateRequest cashTransactionCreateRequest);
        Task<TransactionResponse?> GetTransaction(Guid transactionId);
        Task<List<TransactionResponse>> GetDepositsForDay(DateTime date, Guid accountId);
        Task<List<CashTransactionResponse>> GetCashDepositsForDay(DateTime date, Guid accountId);
        Task<List<ChequeTransactionResponse>> GetChequeDepositsForDay(DateTime date, Guid accountId);
        Task<List<TransactionResponse>> GetWithdrawalsForDay(DateTime date, Guid accountId);
        Task<List<CashTransactionResponse>> GetCashWithdrawalsForDay(DateTime date, Guid accountId);
        Task<List<ChequeTransactionResponse>> GetChequeWithdrawalsForDay(DateTime date, Guid accountId);
        Task<List<DailyTransactionsAggregateResponse>> GetMonthlyTransactionsAggregate(DateTime date, Guid accountId);
        Task<DailyTransactionsAggregateResponse> GetDailyTransactionsAggregate(DateTime date, Guid accountId);
        Task<List<TransactionResponse>> GetTransactionsForAccount(Guid accountId);
        Task<List<TransactionResponse>> SortTransactions(List<TransactionResponse> transactions, string sortBy, SortOrderOptions sortOrder);
    }
}
