using PFMS.DAL.Entities;

namespace PFMS.API.Models
{
    public class TotalTransactionAmountModel
    {
        public Guid TotalTransactionAmountId { get; set; }

        public decimal TotalExpence { get; set; }

        public decimal TotalIncome { get; set; }

        public DateTime LastTransactionDate { get; set; }

    }
}
