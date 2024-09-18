using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Enums;

namespace PFMS.API.Models
{
    public class TransactionCategoryResponseModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
