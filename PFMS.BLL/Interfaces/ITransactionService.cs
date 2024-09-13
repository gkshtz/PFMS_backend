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
        public Task<List<TransactionBo>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort);
    
        public Task<TransactionBo> AddTransaction(TransactionBo transactionBo, Guid userId)
    }
}
