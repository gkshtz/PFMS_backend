using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.DTOs;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Interfaces
{
    public interface ITransactionNotificationsRepository<Dto>: IGenericRepository<Dto>
        where Dto: TransactionNotificationDto
    {
        public Task DeleteAllNotificationsOfUser(Guid userId);
        public Task<List<TransactionNotificationDto>> GetAllNotificationsByUserId(Guid userId);
        public Task<List<TransactionNotificationDto>> GetAllNotificationsOfToday();
    }
}
