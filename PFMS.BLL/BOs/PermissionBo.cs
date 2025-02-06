using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.BLL.BOs
{
    public class PermissionBo: IIdentifiable
    {
        public Guid Id { get; set; }
        public required string PermissionName { get; set; }
    }
}
