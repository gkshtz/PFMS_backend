using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    public class TransactionCategory: IIdentifiable
    {
        [Key]
        [Column("categoryId")]
        public Guid Id { get; set; }

        [Column("categoryName")]
        [Required]
        public string CategoryName { get; set; }

        [Column("transactionType")]
        [Required]
        public string TransactionType { get; set; }
    }
}
