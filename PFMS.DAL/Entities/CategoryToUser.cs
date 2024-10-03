using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.DAL.Entities
{
    public class CategoryToUser
    {
        [Key]
        [ForeignKey("Category")]
        [Column("categoryId")]
        public Guid CategoryId { get; set; }

        [ForeignKey("User")]
        [Column("userId")]
        public Guid? UserId { get; set; }

        #region Navigation Properties
        public User? User { get; set; }
        public TransactionCategory? Category { get; set; }
        #endregion
    }
}
