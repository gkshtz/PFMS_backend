using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Enums;
using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class TransactionCategoryBo: IIdentifiable
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
