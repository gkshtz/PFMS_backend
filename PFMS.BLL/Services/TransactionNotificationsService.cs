﻿using System;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TransactionNotificationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddTransactionNotification(TransactionNotificationBo notificationBo, Guid userId)
        {
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            notificationBo.Id = Guid.NewGuid();
            notificationBo.UserId = userId;
            notificationBo.IsNotificationSent = false;

            var notificationDto = _mapper.Map<TransactionNotificationDto>(notificationBo);

            await _unitOfWork.TransactionNotificationsRepository.AddAsync(notificationDto);

            await _unitOfWork.SaveDatabaseChangesAsync();
        }
        
        public async Task<TransactionNotificationBo> GetNotificationById(Guid notificationId, Guid userId)
        {
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            // check if user exists
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            TransactionNotificationDto? notificationDto = await _unitOfWork.TransactionNotificationsRepository.GetByIdAsync(notificationId);
            // check if the notification exists
            if(notificationDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.TransactionNotificationNotFound);
            }

            var notificationBo = _mapper.Map<TransactionNotificationBo>(notificationDto);
            // check if the the notification belongs to this user
            if(notificationBo.UserId != userId)
            {
                throw new BadRequestException(ErrorMessages.TransactionNotificationDoesNotBelongToUser);
            }

            return notificationBo;
        }
        public async Task<List<TransactionNotificationBo>> GetAllNotificationsOfUser(Guid userId)
        {
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            List<TransactionNotificationDto> notificationDtos = await _unitOfWork.TransactionNotificationsRepository.GetAllNotificationsByUserId(userId);
            return _mapper.Map<List<TransactionNotificationBo>>(notificationDtos);
        }

        public async Task UpdateTransactionNotificationAsync(TransactionNotificationBo notificationBo, Guid notificationId, Guid userId)
        {
            await CheckUserAndNotification(notificationId, userId);

            notificationBo.Id = notificationId;
            notificationBo.UserId = userId;

            var notificationDto = _mapper.Map<TransactionNotificationDto>(notificationBo);
            
            await _unitOfWork.TransactionNotificationsRepository.UpdateAsync(notificationDto);
            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        public async Task DeleteTransactionNotificationAsync(Guid notificationId, Guid userId)
        {
            await CheckUserAndNotification(notificationId, userId);
            await _unitOfWork.TransactionNotificationsRepository.DeleteAsync(notificationId);
            await _unitOfWork.SaveDatabaseChangesAsync();
        }

        private async Task CheckUserAndNotification(Guid notificationId, Guid userId)
        {
            // check if the user exists
            UserDto? userDto = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if(userDto == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.UserNotFound);
            }

            Guid? userIdOfNotification = (await _unitOfWork.TransactionNotificationsRepository.GetByIdAsync(notificationId))?.UserId;
            if(userIdOfNotification  == null)
            {
                throw new ResourceNotFoundExecption(ErrorMessages.TransactionNotificationNotFound);
            }

            if(userIdOfNotification != userId)
            {
                throw new BadRequestException(ErrorMessages.TransactionNotificationDoesNotBelongToUser);
            }
        }
    }
}
