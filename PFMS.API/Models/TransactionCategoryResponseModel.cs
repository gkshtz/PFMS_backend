using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.API.Models
{
    public class TransactionCategoryResponseModel: IIdentifiable
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
