using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class RecurringTransactionResponseModel: RecurringTransactionRequestModel, IIdentifiable
    {
        public Guid Id { get; set; }
        public DateOnly? LastTransactionDate { get; set; }
        public DateOnly NextTransactionDate { get; set; }
        public Guid UserId { get; set; }
    }
}
