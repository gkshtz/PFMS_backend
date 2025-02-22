using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMS.DAL.Data;
using PFMS.DAL.DTOs;
using PFMS.DAL.Entities;
using PFMS.DAL.Interfaces;

namespace PFMS.DAL.Repositories
{
    public class TransactionNotificationsRepository<Dto, Entity>: GenericRepository<Dto, Entity>, ITransactionNotificationsRepository<Dto>
        where Dto: TransactionNotificationDto
        where Entity: TransactionNotification
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public TransactionNotificationsRepository(AppDbContext appDbContext, IMapper mapper): base(appDbContext, mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task DeleteAllNotificationsOfUser(Guid userId)
        {
            List<TransactionNotification> notifications = await _appDbContext.TransactionNotifications.Where(x => x.UserId == userId).ToListAsync();
            _appDbContext.TransactionNotifications.RemoveRange(notifications);
        }

        public async Task<List<TransactionNotificationDto>> GetAllNotificationsByUserId(Guid userId)
        {
            List<TransactionNotification> notifications = await _appDbContext.TransactionNotifications.Where(x => x.UserId == userId).ToListAsync();
            return _mapper.Map<List<TransactionNotificationDto>>(notifications);
        }

        public async Task<List<TransactionNotificationDto>> GetAllNotificationsOfToday()
        {
            List<TransactionNotification> notifications = await _appDbContext.TransactionNotifications
                .Where(x => x.TransactionDate == DateOnly.FromDateTime(DateTime.Today)).ToListAsync();
            return _mapper.Map<List<TransactionNotificationDto>>(notifications);
        }
    }
}
