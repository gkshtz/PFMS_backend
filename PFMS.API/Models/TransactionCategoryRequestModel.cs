using PFMS.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PFMS.Utils.Enums;

namespace PFMS.API.Models
{
    public class TransactionCategoryRequestModel
    {
        public string CategoryName { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}
