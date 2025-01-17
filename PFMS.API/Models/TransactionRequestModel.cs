using PFMS.DAL.Entities;
using PFMS.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Constants;
using PFMS.API.CustomValidators;

namespace PFMS.API.Models
{
    public class TransactionRequestModel
    {
        [Required]
        [StringLength(50, ErrorMessage = ErrorMessages.LongTransactionNameLength)]
        public string TransactionName { get; set; }

        [StringLength(200, ErrorMessage = ErrorMessages.LongTransactionDescriptionLength)]
        public string? TransactionDescription { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TransactionAmount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public Guid TransactionCategoryId { get; set; }

        [ValidateScreenshot]
        public IFormFile? File { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionType), ErrorMessage = ErrorMessages.InvalidTransactionType)]
        public TransactionType TransactionType { get; set; }
    }
}
