﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.Entities;
using PFMS.Utils.Enums;
using Microsoft.AspNetCore.Http;
using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class TransactionBo: IIdentifiable
    { 
        public Guid Id { get; set; }
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid TransactionCategoryId { get; set; }
        public TransactionType TransactionType { get; set; }
        public IFormFile? File { get; set; }
        public Guid TotalTransactionAmountId { get; set; }

        #region Navigation Properties
        public TotalTransactionAmountBo? TotalTransactionAmount { get; set; }
        public TransactionCategoryBo? TransactionCategory { get; set; }
        #endregion
    }
}
