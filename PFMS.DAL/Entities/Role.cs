using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFMS.DAL.Entities
{
    public class Role
    {
        [Key]
        [Column("roleId")]
        public Guid RoleId { get; set; }

        [Column("roleName")]
        public string RoleName { get; set; }
    }
}
