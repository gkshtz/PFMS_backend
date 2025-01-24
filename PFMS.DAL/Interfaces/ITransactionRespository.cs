using PFMS.DAL.DTOs;
using PFMS.Utils.RequestData;

namespace PFMS.DAL.Interfaces
{
    public interface ITransactionRepository<Dto>: IGenericRepository<Dto>
        where Dto: TransactionDto
    {
        public Task<List<TransactionDto>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort, Pagination pagination);

        public Task<TotalTransactionAmountDto> GetTotalTransactionAmountByUserId(Guid userId);

        public Task<TransactionDto> AddTransaction(TransactionDto transactionDto);

        public Task<TransactionDto?> GetByTransactionId(Guid transactionId, Guid userId);

        public Task<bool> UpdateTransaction(TransactionDto transactionDto, Guid transactionId, Guid totalTransactionAmountId);

        public Task<bool> DeleteTransaction(Guid transactionId, Guid totalTransactionAmountId);

        public Task<TransactionDto> GetTransactionWithLatestDate(Guid totalTransactionAmountd);
    }

}
