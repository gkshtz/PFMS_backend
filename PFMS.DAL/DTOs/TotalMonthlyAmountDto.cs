using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.DAL.Entities;

namespace PFMS.DAL.DTOs
{
    public class TotalMonthlyAmountDto
    {
        public Guid TotalMonthlyAmountId { get; set; }

        public decimal TotalExpenceOfMonth { get; set; }

        public decimal TotalIncomeOfMonth { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public Guid TotalTransactionAmountId { get; set; }

        #region Navigation properties
        public TotalTransactionAmount? TotalTransactionAmount { get; set; }
        #endregion
    }
}
