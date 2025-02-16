using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class TransactionNotificationResponseModel: IIdentifiable
    {
        public Guid Id { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string? Message { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
