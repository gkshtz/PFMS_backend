using PFMS.Utils.Enums;

namespace PFMS.API.Models
{
    public class TransactionResponseModel
    {
        public Guid TransactionId { get; set; }
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }

        #region Navigation Properties
        public TransactionCategoryResponseModel? TransactionCategory { get; set; }
        public TotalTransactionAmountModel? TotalTransactionAmount { get; set; }
        #endregion
    }
}
