using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.DTOs
{
    public class TransactionNotificationDto: IIdentifiable
    {
        public Guid Id { get; set; }

        public decimal TransactionAmount { get; set; }

        public DateOnly TransactionDate { get; set; }

        public Guid UserId { get; set; }

        public string Message { get; set; }

        public string TransactionType { get; set; }

        public bool IsNotificationSent { get; set; }
    }
}
