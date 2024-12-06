using System.ComponentModel.DataAnnotations;
using PFMS.DAL.Entities;
using PFMS.Utils.Constants;
using PFMS.Utils.Enums;

namespace PFMS.API.Models
{
    public class TransactionCategoryRequestModel
    {
        [Required]
        [StringLength(20)]
        public string CategoryName { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionCategory), ErrorMessage =ErrorMessages.InvalidTransactionType)]
        public TransactionType TransactionType { get; set; }
    }
}
