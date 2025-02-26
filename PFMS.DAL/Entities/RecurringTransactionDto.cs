using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    public class RecurringTransactionDto: IIdentifiable
    {
        public Guid Id { get; set; }
        public string TransactionName { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? LastTransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string TransactionInterval { get; set; }
        public Guid UserId { get; set; }
        public Guid TransactionCategoryId { get; set; }
    }
}
