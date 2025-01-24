using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class BudgetBo: IIdentifiable
    {
        public Guid  Id { get; set; }
        public decimal BudgetAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal SpentPercentage { get; set; }
        public Guid UserId { get; set; }
    }
}
