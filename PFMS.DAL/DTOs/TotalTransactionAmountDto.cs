using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.DTOs
{
    public class TotalTransactionAmountDto: IIdentifiable
    {
        public Guid Id { get; set; }
        public decimal TotalExpence { get; set; }
        public decimal TotalIncome { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public Guid UserId { get; set; }
    }
}
