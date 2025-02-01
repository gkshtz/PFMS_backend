using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMS.Utils.Interfaces;
using PFMS.DAL.Entities;

namespace PFMS.DAL.Entities
{
    public class Role: IIdentifiable
    {
        [Key]
        [Column("roleId")]
        public Guid Id { get; set; }

        [Column("roleName")]
        public string RoleName { get; set; }

        #region Navigation Properties
        public ICollection<Permission> Permissions { get; set; }
        #endregion
    }
}
