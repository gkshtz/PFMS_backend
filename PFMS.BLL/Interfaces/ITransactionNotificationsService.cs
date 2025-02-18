using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PFMS.BLL.BOs;

namespace PFMS.BLL.Interfaces
{
    public interface ITransactionNotificationsService
    {
        public Task AddTransactionNotification(TransactionNotificationBo notificationBo, Guid userId);
        public Task<List<TransactionNotificationBo>> GetAllNotificationsOfUser(Guid userId);
        public Task<TransactionNotificationBo> GetNotificationById(Guid notificationId, Guid userId);
        public Task UpdateTransactionNotificationAsync(TransactionNotificationBo notificationBo, Guid notificationId, Guid userId);
        public Task DeleteTransactionNotificationAsync(Guid notificationId, Guid userId);
    }
}
