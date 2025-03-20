using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Enums;
using PFMS.API.CustomValidators;
using PFMS.Utils.Constants;

namespace PFMS.API.Models
{
    public class RecurringTransactionRequestModel
    {
        [Required]
        [Range(0, Double.MaxValue)]
        public decimal TransactionAmount { get; set; }

        [Required]
        [StringLength(30)]
        public string TransactionName { get; set; }

        [StringLength(50)]
        public string? TransactionDescription { get; set; }

        [Required]
        [DateGreaterThanToday(ErrorMessage = ErrorMessages.DateMustBeGreaterThanToday)]
        public DateOnly StartDate { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionType))]
        public TransactionInterval TransactionInterval { get; set; }

        [Required]
        public Guid TransactionCategoryId { get; set; }
    }
}
