using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PFMS.Utils.Interfaces;

namespace PFMS.DAL.Entities
{
    public class Permission: IIdentifiable
    {
        [Key]
        [Column("permissionId")]
        public Guid Id { get; set; }

        [Column("permissionName")]
        public string PermissionName { get; set; }

        #region Navigation Properties
        public ICollection<Role> Roles { get; set; }
        #endregion
    }
}
