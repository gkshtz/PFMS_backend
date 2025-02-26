using PFMS.Utils.Enums;

namespace PFMS.API.Models
{
    public class RecurringTransactionRequestModel
    {
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }    
        public DateOnly StartDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public RecurringTransactionIntervals TransactionIntervals { get; set; }
        public Guid TransactionCategoryId { get; set; }
    }
}
