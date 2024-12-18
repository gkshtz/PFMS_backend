﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFMS.DAL.Entities
{
    public class TotalTransactionAmount
    {
        [Key]
        [Column("totalTransactionAmountId")]
        public Guid TotalTransactionAmountId { get; set; }

        [Column("totalExpence")]
        public decimal TotalExpence { get; set; }

        [Column("totalIncome")]
        public decimal TotalIncome { get; set; }

        [Column("lastTransactionDate")]
        public DateTime LastTransactionDate { get; set; }

        [Column("UserId")]
        public Guid UserId { get; set; }

        #region Navigation Properties
        public User? User { get; set; }
        #endregion
    }
}
