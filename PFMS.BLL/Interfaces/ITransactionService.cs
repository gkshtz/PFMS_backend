using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.BLL.BOs;
using PFMS.Utils.Request_Data;

namespace PFMS.BLL.Interfaces
{
    public interface ITransactionService
    {
        public Task<List<TransactionBo>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort, Pagination pagination);

        public Task<TransactionBo> AddTransaction(TransactionBo transactionBo, Guid userId);

        public Task<TransactionBo> GetByTransactionId(Guid transactionId, Guid userId);

        public Task UpdateTransaction(TransactionBo transactionBo, Guid userId, Guid transactionId);

        public Task DeleteTransaction(Guid transactionId, Guid userId);
    }
}
