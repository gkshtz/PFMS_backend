using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;

namespace PFMS.BLL.BOs
{
    public class TransactionCategoryBo
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
