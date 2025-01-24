using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class RoleBo: IIdentifiable
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
