using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class TransactionNotificationBo: IIdentifiable
    {
        public Guid Id { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public TransactionType TransactionType { get; set; }
        public bool IsNotificationSent { get; set; }
    }
}
