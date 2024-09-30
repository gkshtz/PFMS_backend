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
    public class TransactionCategory
    {
        [Key]
        [Column("categoryId")]
        public Guid CategoryId { get; set; }

        [Column("categoryName")]
        [Required]
        public string CategoryName { get; set; }

        [Column("transactionType")]
        [Required]
        public string TransactionType { get; set; }

        [ForeignKey("User")]
        [Column("userId")]
        public Guid? UserId { get; set; }

        #region Navigation Properties
        public User? User { get; set; }
        #endregion
    }
}
