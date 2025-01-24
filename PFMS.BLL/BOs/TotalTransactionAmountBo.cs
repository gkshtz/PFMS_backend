using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class TotalTransactionAmountBo: IIdentifiable
    {
        public Guid Id { get; set; }
        public decimal TotalExpence { get; set; }
        public decimal TotalIncome { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public Guid UserId { get; set; }

        public TotalTransactionAmountBo()
        {
            LastTransactionDate = DateTime.UtcNow;
            TotalExpence = 0;
            TotalIncome = 0;   
        }
    }
}
