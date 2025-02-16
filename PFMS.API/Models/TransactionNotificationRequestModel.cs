using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Constants;
using PFMS.Utils.Enums;
using PFMS.API.CustomValidators;
namespace PFMS.API.Models
{
    public class TransactionNotificationRequestModel
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = ErrorMessages.TransactionAmountMustBeGreaterThan1)]
        public decimal TransactionAmount { get; set; }

        [Required]
        [DateGreaterThanToday(ErrorMessage = ErrorMessages.DateMustBeGreaterThanToday)]
        public DateOnly TransactionDate { get; set; }

        [StringLength(100)]
        public string? Message { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionType), ErrorMessage = ErrorMessages.InvalidTransactionType)]
        public TransactionType TransactionType { get; set; }
    }
}
