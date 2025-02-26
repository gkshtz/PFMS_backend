using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.BLL.BOs;
using PFMS.BLL.Interfaces;
using PFMS.DAL.DTOs;
using PFMS.DAL.Interfaces;
using PFMS.Utils.CustomExceptions;
using PFMS.Utils.Constants;
using AutoMapper;
using PFMS.DAL.Entities;
namespace PFMS.BLL.Services
{
    public class RecurringTransactionsService: IRecurringTransactionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RecurringTransactionsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task AddRecurringTransaction(RecurringTransactionBo recurringTransactionBo, Guid userId)
        {
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            recurringTransactionBo.Id = Guid.NewGuid();
            recurringTransactionBo.UserId = userId;

            var recurringTransactionDto = _mapper.Map<RecurringTransactionDto>(recurringTransactionBo);
            await _unitOfWork.RecurringTransactionsRepository.AddAsync(recurringTransactionDto);
            await _unitOfWork.SaveDatabaseChangesAsync();
        }
    }
}
