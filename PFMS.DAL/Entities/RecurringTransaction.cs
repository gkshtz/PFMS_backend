using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    public class RecurringTransaction: IIdentifiable
    {
        [Key]
        [Column("recrurringTransactionId")]
        public Guid Id { get; set; }

        [Column("transactionName")]
        public string TransactionName { get; set; }

        [Column("transactionDescription")]
        public string? TransactionDescription { get; set; }

        [Column("transactionAmount")]
        public decimal TransactionAmount { get; set; }

        [Column("startDate")]
        public DateOnly StartDate { get; set; }

        [Column("transactionType")]
        public string TransactionType { get; set; }

        [Column("transactionInterval")]
        public string TransactionInterval { get; set; }

        [Column("lastTransactionDate")]
        public DateOnly? LastTransactionDate { get; set; }

        [Column("userId")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Column("transactionCategoryId")]
        [ForeignKey("TransactionCategory")]
        public Guid TransactionCategoryId { get; set; }
        
        #region Navigation Properties
        public User? User { get; set; }
        public TransactionCategory? TransactionCategory { get; set; }
        #endregion
    }
}
