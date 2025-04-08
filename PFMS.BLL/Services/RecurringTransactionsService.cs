using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Constants;
using System.Transactions;
using PFMS.Utils.Enums;

namespace PFMS.BLL.Services
{
    public class RecurringTransactionsService: IRecurringTransactionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RecurringTransactionsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task AddRecurringTransaction(RecurringTransactionBo recurringTransactionBo, Guid userId)
        {
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            recurringTransactionBo.Id = Guid.NewGuid();
            recurringTransactionBo.UserId = userId;
            recurringTransactionBo.LastTransactionDate = null;
            recurringTransactionBo.NextTransactionDate = recurringTransactionBo.StartDate;

            var recurringTransactionDto = _mapper.Map<RecurringTransactionDto>(recurringTransactionBo);
            await _unitOfWork.RecurringTransactionsRepository.AddAsync(recurringTransactionDto);
            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task<List<RecurringTransactionBo>> GetAllRecurringTransactions(Guid userId)
        {
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            IEnumerable<RecurringTransactionDto> recurringTransactionDtos = await _unitOfWork.RecurringTransactionsRepository.GetAllRecurringTransactions(userId);
            return _mapper.Map<List<RecurringTransactionBo>>(recurringTransactionDtos);
        }
    }
}
