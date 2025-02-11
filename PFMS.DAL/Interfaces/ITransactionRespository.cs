using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PFMS.DAL.DTOs;
using PFMS.Utils.RequestData;

namespace PFMS.DAL.Interfaces
{
    public interface ITransactionRepository<Dto>: IGenericRepository<Dto>
        where Dto: TransactionDto
    {
        public Task<List<TransactionDto>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort, Pagination pagination);

        public Task<TotalTransactionAmountDto> GetTotalTransactionAmountByUserId(Guid userId);

        public Task<TransactionDto> GetTransactionWithLatestDate(Guid totalTransactionAmountd);

        public Task DeleteTransactionsByTotalTransactionAmountId(Guid totalTransactionAmountId);
    }

}
