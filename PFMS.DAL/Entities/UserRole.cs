using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PFMS.DAL.Entities
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        [Column("userId")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Column("roleId")]
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        
        #region Navigation Properties
        public Role? Role { get; set; }
        public User? User { get; set; }
        #endregion
    }
}
