using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;
 
namespace PFMS.DAL.Entities
{
    public class Transaction
    {
        [Column("transactionId")]
        [Key]
        public Guid TransactionId { get; set; }
        [Column("transactionName")]
        public string TransactionName { get; set; }
        [Column("transactionDescription")]
        public string? TransactionDescription { get; set; }
        [Column("transactionAmount")]
        public decimal TransactionAmount { get; set; }
        [Column("transactionDate")]
        public DateTime TransactionDate { get; set; }

        [ForeignKey("TransactionCategory")]
        [Column("transactionCategoryId")]
        public Guid TransactionCategoryId { get; set; }
        [Column("transactionType")]
        public TransactionType TransactionType { get; set; }

        [ForeignKey("TotalTransactionAmount")]
        [Column("totalTransactionAmountId")]
        public Guid TotalTransactionAmountId { get; set; }

        #region Navigation Properties
        public TransactionCategory? TransactionCategory { get; set; }
        public TotalTransactionAmount? TotalTransactionAmount { get; set; }
        #endregion
    }
}
