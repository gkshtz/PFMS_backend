using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.BLL.BOs
{
    public class TotalTransactionAmountBo
    {
        public Guid TotalTransactionAmountId { get; set; }
        public decimal TotalExpence { get; set; }
        public decimal TotalIncome { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public Guid UserId { get; set; }
    }
}
