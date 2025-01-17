using Microsoft.AspNetCore.Http;
using PFMS.BLL.BOs;
using PFMS.Utils.RequestData;

namespace PFMS.BLL.Interfaces
{
    public interface ITransactionService
    {
        public Task<List<TransactionBo>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort, Pagination pagination);

        public Task<Guid> AddTransaction(TransactionBo transactionBo, Guid userId, IFormFile? file, string rootPath);

        public Task<TransactionBo> GetByTransactionId(Guid transactionId, Guid userId);

        public Task UpdateTransaction(TransactionBo transactionBo, Guid userId, Guid transactionId, string rootPath);

        public Task DeleteTransaction(Guid transactionId, Guid userId);

        public Task<TotalTransactionAmountBo> GetTotalTransactionAmountAsync(Guid userId);

        public Task<TotalMonthlyAmountBo> GetPreviousMonthSummary(int month, int year, Guid userId);
    }
}
