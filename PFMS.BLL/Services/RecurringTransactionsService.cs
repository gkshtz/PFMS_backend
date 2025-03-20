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

            recurringTransactionBo.UserId = userId;
        }
    }
}
