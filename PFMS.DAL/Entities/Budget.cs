﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    [Index(nameof(Month), nameof(Year), IsUnique = true)]
    public class Budget: IIdentifiable
    {
        [Key]
        [Column("budgetId")]
        public Guid Id { get; set; }

        [Column("budgetAmount")]
        public decimal BudgetAmount { get; set; }

        [Column("month")]
        public int Month { get; set; }

        [Column("year")]
        public int Year { get; set; }

        [ForeignKey("UserId")]
        [Column("userId")]
        public Guid UserId { get; set; }
        
        #region Navigation Properties
        public User? User { get; set; }
        #endregion
    }
}
