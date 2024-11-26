using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.DAL.DTOs
{
    public class UserRoleDto
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
    }
}
