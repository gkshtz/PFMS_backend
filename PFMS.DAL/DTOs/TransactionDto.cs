﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;

namespace PFMS.DAL.DTOs
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid TransactionCategoryId { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid TotalTransactionAmountId { get; set; }
    }
}
