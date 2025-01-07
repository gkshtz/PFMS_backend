namespace PFMS.API.Models
{
    public class MonthlySummaryResponseModel
    {
        public decimal TotalExpenceOfMonth { get; set; }
        public decimal TotalIncomeOfMonth { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal? BudgetAmount { get; set; }
    }
}
