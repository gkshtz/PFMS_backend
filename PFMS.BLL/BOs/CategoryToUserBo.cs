using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.BLL.BOs
{
    public class CategoryToUserBo
    {
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
