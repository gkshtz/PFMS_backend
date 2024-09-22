using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;
using PFMS.Utils.Request_Data;

namespace PFMS.DAL.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<List<TransactionDto>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort, Pagination pagination);

        public Task<TotalTransactionAmountDto> GetTotalTransactionAmountByUserId(Guid userId);

        public Task<TransactionDto> AddTransaction(TransactionDto transactionDto);

        public Task<TransactionDto?> GetByTransactionId(Guid transactionId, Guid userId);

        public Task<bool> UpdateTransaction(TransactionDto transactionDto, Guid transactionId, Guid totalTransactionAmountId);
    }

}
