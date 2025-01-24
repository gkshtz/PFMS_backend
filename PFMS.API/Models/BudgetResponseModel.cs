using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class BudgetResponseModel: IIdentifiable
    {
        public Guid Id { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal SpentPercentage { get; set; }
    }
}
