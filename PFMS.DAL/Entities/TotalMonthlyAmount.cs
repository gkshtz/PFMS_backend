﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.DAL.Entities
{
    public class TotalMonthlyAmount
    {
        [Column("totalMonthlyAmountId")]
        [Key]
        public Guid TotalMonthlyAmountId { get; set; }

        [Column("totalExpenceOfMonth")]
        public decimal TotalExpenceOfMonth { get; set; }

        [Column("totalIncomeOfMonth")]
        public decimal TotalIncomeOfMonth { get; set; }

        [Column("month")]
        public int Month { get; set; }

        [Column("year")]
        public int Year { get; set; }

        [Column("totalTransactionAmountId")]
        public Guid TotalTransactionAmountId { get; set; }

        #region Navigation properties
        public TotalTransactionAmount? TotalTransactionAmount { get; set; }
        #endregion
    }
}
