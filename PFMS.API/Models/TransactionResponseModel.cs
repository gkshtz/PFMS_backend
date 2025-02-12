using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class TransactionResponseModel: IIdentifiable
    {
        public Guid Id { get; set; }
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid TransactionCategoryId { get; set; }

        #region Navigation Properties
        public TransactionCategoryResponseModel? TransactionCategory { get; set; }
        public TotalTransactionAmountModel? TotalTransactionAmount { get; set; }
        #endregion
    }
}
