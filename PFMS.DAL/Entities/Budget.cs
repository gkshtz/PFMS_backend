using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.DAL.Entities
{
    public class Budget
    {
        [Key]
        [Column("budgetId")]
        public Guid BudgetId { get; set; }

        [Column("budgetAmount")]
        public decimal BudgetAmount { get; set; }

        [Column("month")]
        public int Month { get; set; }

        [Column("year")]
        public int Year { get; set; }

        [ForeignKey("User")]
        [Column("userId")]
        public Guid UserId { get; set; }
        
        #region Navigation Properties
        public User? User { get; set; }
        #endregion
    }
}
