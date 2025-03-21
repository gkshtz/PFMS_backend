using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.DTOs
{
    public class RecurringTransactionDto: IIdentifiable
    {
        public Guid Id { get; set; }
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionInterval TransactionInterval { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly LastTransactionDate { get; set; }
        public Guid UserId { get; set; }
        public Guid TransactionCategoryId { get; set; }
    }
}
