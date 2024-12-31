namespace PFMS.API.Models
{
    public class BudgetResponseModel
    {
        public Guid  BudgetId { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal SpentPercentage { get; set; }
    }
}
