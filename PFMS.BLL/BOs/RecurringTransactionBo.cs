using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class RecurringTransactionBo : IIdentifiable
    {
        public Guid Id { get; set; }
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? LastTransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionInterval TransactionInterval { get; set; }
        public Guid TransactionCategoryId { get; set; }
        public Guid UserId { get; set; }
    }
}
