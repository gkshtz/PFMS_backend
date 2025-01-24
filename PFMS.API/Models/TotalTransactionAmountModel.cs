using PFMS.DAL.Entities;
using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class TotalTransactionAmountModel: IIdentifiable
    {
        public Guid Id { get; set; }

        public decimal TotalExpence { get; set; }

        public decimal TotalIncome { get; set; }

        public DateTime LastTransactionDate { get; set; }

    }
}
