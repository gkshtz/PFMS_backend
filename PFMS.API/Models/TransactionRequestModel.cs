using PFMS.DAL.Entities;
using PFMS.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PFMS.API.Models
{
    public class TransactionRequestModel
    {
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid TransactionCategoryId { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
