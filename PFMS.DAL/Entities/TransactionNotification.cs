﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    public class TransactionNotification: IIdentifiable
    {
        [Key]
        [Column("notificationId")]
        public Guid Id { get; set; }

        [Column("transactionAmount")]
        public decimal TransactionAmount { get; set; }

        [Column("transactionDate")]
        public DateOnly TransactionDate { get; set; }

        [ForeignKey("User")]
        [Column("userId")]
        public Guid UserId { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("transactionType")]
        public string TransactionType { get; set; }

        [Column("isNotificationSent")]
        public bool IsNotificationSent { get; set; }

        #region Navigation Properties
        public User? User { get; set; }
        #endregion
    }
}
