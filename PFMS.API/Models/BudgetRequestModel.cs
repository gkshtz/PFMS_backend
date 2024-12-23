using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Constants;

namespace PFMS.API.Models
{
    public class BudgetRequestModel
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = ErrorMessages.BudgetAmountRageExceeded)]
        public decimal BudgetAmount { get; set; }

        [Required]
        [Range(1,12, ErrorMessage = ErrorMessages.InvalidMonth)]
        public int Month { get; set; }

        [Required]
        [Range(2000, 3000, ErrorMessage = ErrorMessages.InvalidYear)]
        public int Year { get; set; }
    }
}
