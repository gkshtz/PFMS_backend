using AutoMapper;
using Microsoft.Identity.Client;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.Custom_Exceptions;
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
        public async Task<List<TransactionBo>> GetAllTransactionsAsync(Guid userId, Filter? filter, Sort? sort)
        {
            List<TransactionDto> transactionsDto = await _transactionRepository.GetAllTransactionsAsync(userId, filter, sort);
            return _mapper.Map<List<TransactionBo>>(transactionsDto);
        }

        public async Task<TransactionBo> AddTransaction(TransactionBo transactionBo, Guid userId)
        {
            var totalTransactionAmountDto = await _transactionRepository.GetTotalTransactionAmountByUserId(userId);

            if(totalTransactionAmountDto == null)
            {
                throw new ResourceNotFoundExecption();
            }

            transactionBo.TotalTransactionAmountId = totalTransactionAmountDto.TotalTransactionAmountId;
            var transactionDto = _mapper.Map<TransactionDto>(transactionBo);
            transactionDto = await _transactionRepository.AddTransaction(transactionDto);

            return _mapper.Map<TransactionBo>(transactionDto);
        }
    }
}
