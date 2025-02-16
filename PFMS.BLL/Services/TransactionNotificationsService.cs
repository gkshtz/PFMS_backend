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
    public class TransactionNotificationsService: ITransactionNotificationsService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        public TransactionNotificationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitofWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddTransactionNotification(TransactionNotificationBo notificationBo, Guid userId)
        {
            UserDto? userDto = await _unitofWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            notificationBo.Id = Guid.NewGuid();
            notificationBo.UserId = userId;
            notificationBo.IsNotificationSent = false;

            var notificationDto = _mapper.Map<TransactionNotificationDto>(notificationBo);

            await _unitofWork.TransactionNotificationsRepository.AddAsync(notificationDto);

            await _unitofWork.SaveDatabaseChangesAsync();
        }
    }
}
