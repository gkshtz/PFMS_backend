using AutoMapper;
using Microsoft.Identity.Client;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Request_Data;

namespace PFMS.BLL.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<List<TransactionBo>> GetAllTransactionsAsync(Guid userId, Filter? filter)
        {
            List<TransactionDto> transactionsDto = await _transactionRepository.GetAllTransactionsAsync(userId, filter);            
            return _mapper.Map<List<TransactionBo>>(transactionsDto);
        }
    }
}
