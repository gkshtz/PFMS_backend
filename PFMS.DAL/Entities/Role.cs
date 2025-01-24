using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    public class Role: IIdentifiable
    {
        [Key]
        [Column("roleId")]
        public Guid Id { get; set; }

        [Column("roleName")]
        public string RoleName { get; set; }
    }
}
